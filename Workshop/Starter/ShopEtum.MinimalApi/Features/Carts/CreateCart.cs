using ShopEtum.MinimalApi.Shared.Slices;
using ShopEtum.MinimalApi.Shared.Domain.Entities;
using ShopEtum.MinimalApi.Shared.Persistence;
using ShopEtum.MinimalApi.Shared.Security;

namespace ShopEtum.MinimalApi.Features.Carts;

public class CreateCart : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/carts",
            async (ShopEtumDbContext db,
                ICurrentUserService currentUserService,
                CancellationToken cancellationToken) =>
            {
                var cart = new Cart(currentUserService.UserId);

                db.Carts.Add(cart);
                await db.SaveChangesAsync(cancellationToken);

                var createdCart = new
                {
                    cart.Id,
                    cart.CustomerIdentifier,
                    cart.CreatedDate,
                    cart.ModifiedDate
                };

                return Results.Created($"/carts/{cart.Id}", createdCart);
            }).WithName("CreateCart");
    }
}  