using BFFSample.ClientWithLocalAPI.Models.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddOpenIdConnectAccessTokenManagement();
builder.Services.AddHttpClient("RemoteApiClient", config =>
{
    config.BaseAddress = new Uri(builder.Configuration["RemoteApiRoot"] ??
        throw new ArgumentNullException());
}).AddUserAccessTokenHandler();

var entraIdOpenIdConnectSettings = builder.Configuration.GetSection("EntraIdOpenIdConnect");

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.HttpOnly = true;
    // Cookies must be sent during cross-site requests when integrating with an IDP.
    // Lax is the default, None can be required depending on the flow/IDP/functionality 
    // you're using.
    options.Cookie.SameSite = SameSiteMode.Lax;
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
    options.Scope.Add(entraIdOpenIdConnectSettings["RemoteApiScope"] ?? "");

    options.TokenValidationParameters.NameClaimType = "name";
    options.TokenValidationParameters.RoleClaimType = "role";
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
 

app.MapGet("/proxytoremoteapi/hello", async (HttpContext httpContext, 
    IHttpClientFactory httpClientFactory) =>
{
    // call the remote api 
    var httpClient = httpClientFactory.CreateClient("RemoteApiClient");
    var request = new HttpRequestMessage(HttpMethod.Get, "/remoteapi/hello");
    var response = await httpClient.SendAsync(request, 
        HttpCompletionOption.ResponseHeadersRead, 
        httpContext.RequestAborted);

    response.EnsureSuccessStatusCode();

    using (var responseStream = await response.Content.ReadAsStreamAsync())
    { 
        // Complete the endpoint implementation
        var responseMsg = await JsonSerializer.DeserializeAsync<RemoteApiResponse>(
            responseStream,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (responseMsg is null)
        {
            return Results.StatusCode(500);
        }

        return Results.Ok(responseMsg);
    }  
}).RequireAuthorization();


app.Run();
