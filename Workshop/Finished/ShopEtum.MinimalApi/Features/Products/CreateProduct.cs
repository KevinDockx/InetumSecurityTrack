using ShopEtum.MinimalApi.Shared.Slices;
using ShopEtum.MinimalApi.Shared.Domain.Entities;
using ShopEtum.MinimalApi.Shared.Persistence;

namespace ShopEtum.MinimalApi.Features.Products;

public class CreateProduct : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/products",
            async (ProductForCreationDto productDto, 
                ShopEtumDbContext db, 
                CancellationToken cancellationToken) =>
        {
            // note: demo code!  IRL, include a validation step! 

            var product = new Product(productDto.Name, 
                productDto.Description, 
                productDto.Price, 
                DateTime.UtcNow);

            db.Products.Add(product);
            await db.SaveChangesAsync(cancellationToken);

            return Results.Created($"/products/{product.Id}", 
                new
                {
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price
                });
        }).WithName("CreateProduct")
        .RequireAuthorization(); ;
    }

    public class ProductForCreationDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; } 
        public decimal Price { get; set; } 
    }


}
