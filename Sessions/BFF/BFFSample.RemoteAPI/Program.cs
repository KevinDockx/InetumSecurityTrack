var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters.NameClaimType = "unique_name";
        options.TokenValidationParameters.RoleClaimType = "role";   
    });
builder.Services.AddAuthorization();

var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// not necessary anymore for minimal APIs, added by default.
//app.UseAuthentication(); 
//app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
}
app.UseStatusCodePages();

app.MapGet("/remoteapi/hello", (HttpContext httpContext) =>
{ 
    return Results.Ok(new
    {
        Message = "Hello from the remote API!  It seems you are:",
        Claims = httpContext.User.Claims.Select(c => new { c.Type, c.Value })
    }); 
}).RequireAuthorization(); ;

app.Run();
 