using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using ShopEtum.MinimalApi.Shared.Persistence;
using ShopEtum.MinimalApi.Shared.Security;
using ShopEtum.MinimalApi.Shared.Slices;

var builder = WebApplication.CreateBuilder(args); 
IdentityModelEventSource.ShowPII = true;
IdentityModelEventSource.LogCompleteSecurityArtifact = true;

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ShopEtumDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopEtumDbConnection")));

builder.Services.AddProblemDetails();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.RegisterSlices(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
}
app.UseStatusCodePages();

app.MapSliceEndpoints();

app.Run();
 