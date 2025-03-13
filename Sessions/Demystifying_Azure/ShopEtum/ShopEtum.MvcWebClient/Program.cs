using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddOpenIdConnectAccessTokenManagement();
builder.Services.AddHttpClient("ApiClient", config =>
{
    config.BaseAddress = new Uri(builder.Configuration["ApiRoot"] ??
        throw new ArgumentNullException());
}).AddUserAccessTokenHandler();

var entraIdOpenIdConnectSettings = builder.Configuration.GetSection("EntraIdOpenIdConnect");

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
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
    // Entra does not automatically return resource scopes, except the user impersonatio scope when
    // another resource scope is requested that implies user impersonation (delegation: the app should
    // be allowed to acces the API on behalf of the user).  Request it/them explicitly.
    options.Scope.Add("api://b511b135-f73d-433a-8cac-fcc14ccf82c3/shopetumapi.fullaccess");

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
    pattern: "{controller=Products}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
