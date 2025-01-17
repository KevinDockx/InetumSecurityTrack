using ShopEtum.MinimalApi.Shared.Slices;
using ShopEtum.MinimalApi.Shared.Domain.Entities;
using ShopEtum.MinimalApi.Shared.Persistence;

namespace ShopEtum.MinimalApi.Features.Products;

public class GetProduct : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/products/{id}", 
            async (int id, 
                ShopEtumDbContext db, 
                HttpContext ctx,
                CancellationToken cancellationToken) =>
        {
            var user = ctx.User;
            return await db.Products.FindAsync([id], cancellationToken)
                is Product product
                    ? Results.Ok(new
                    {
                        product.Id,
                        product.Name,
                        product.Description,
                        product.Price
                    })
                    : Results.NotFound();
        }).RequireAuthorization().WithName("GetProductById");
    }
}
