using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Net.Http.Headers;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var entraIdOpenIdConnectSettings = builder.Configuration.GetSection("EntraIdOpenIdConnect");

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
 {
     options.Cookie.HttpOnly = true;
     // Cookies must be sent during cross-site requests when integrating with an IDP.
     // Lax is the default, None can be required depending on the flow/IDP/functionality 
     // you're using.
     options.Cookie.SameSite = SameSiteMode.None;
     options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
 })
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.MapInboundClaims = false;
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.Authority = entraIdOpenIdConnectSettings["Authority"];
    options.ClientId = entraIdOpenIdConnectSettings["ClientId"];
    options.ClientSecret = entraIdOpenIdConnectSettings["ClientSecret"];
    options.ResponseType = "code";
    options.SaveTokens = true;

    // Entra automatically returns identity claims configured in your identity & access token configuration! No need to
    // request the related scopes them explicitly. Adding/removing them here will have no effect. 
    // options.Scope.Remove("email"); => no effect
    // options.Scope.Add("ctry"); => no effect

    // Entra does not automatically return a refresh token.  Request it explicitly.
    options.Scope.Add("offline_access");

    // Entra does not automatically return resource scopes, except the user impersonation scope when
    // another resource scope is requested that implies user impersonation (delegation: the app should
    // be allowed to acces the API on behalf of the user).  Request it/them explicitly.
    options.Scope.Add(entraIdOpenIdConnectSettings["RemotApiScope"] ?? "");

    options.TokenValidationParameters.NameClaimType = "name";
    options.TokenValidationParameters.RoleClaimType = "role";
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(builderContext =>
    {
        builderContext.AddRequestTransform(async transformContext =>
        {
            // Forward the access token to the remote API - IRL, add token refresh if needed!
            var accessToken = await transformContext.HttpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(accessToken))
            {
                transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapGet("/minimallocalapi/hello", (HttpContext httpContext) =>
{
    return Results.Ok(new
    {
        Message = "Hello from the minimal local API!  It seems you are:",
        Claims = httpContext.User.Claims.Select(c => new { c.Type, c.Value })
    });
}).RequireAuthorization();

app.MapReverseProxy();

app.Run();
