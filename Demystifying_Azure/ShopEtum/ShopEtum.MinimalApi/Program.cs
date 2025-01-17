using Microsoft.EntityFrameworkCore;
using ShopEtum.MinimalApi.Shared.Slices;
using ShopEtum.MinimalApi.Shared.Persistence;
using ShopEtum.MinimalApi.Shared.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ShopEtumDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopEtumDbConnection")));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer();
builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapSliceEndpoints();

app.Run();
 