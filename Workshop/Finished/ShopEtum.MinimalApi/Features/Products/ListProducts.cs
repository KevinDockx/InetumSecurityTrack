using Microsoft.EntityFrameworkCore;
using ShopEtum.MinimalApi.Shared.Slices;
using ShopEtum.MinimalApi.Shared.Persistence;

namespace ShopEtum.MinimalApi.Features.Products;

public class ListProducts : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/products", 
            async (ShopEtumDbContext db, CancellationToken cancellationToken) =>
        {
            var products = await db.Products
                .Select(product => new
                {
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price
                })
                .ToListAsync(cancellationToken);

            return Results.Ok(products);
        }).WithName("GetProducts")
        .RequireAuthorization(); 
    }

}
