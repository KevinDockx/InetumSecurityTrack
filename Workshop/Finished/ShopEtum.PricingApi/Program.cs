using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddAuthentication()
    .AddJwtBearer();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
 

app.MapGet("/productprices/{id}", (int id) =>
{
    // return a random product price
    var random = new Random();
    var price = random.Next(1, 100);
    return Results.Ok(new {Id = id, Price = price });
}).RequireAuthorization();

app.Run();
 