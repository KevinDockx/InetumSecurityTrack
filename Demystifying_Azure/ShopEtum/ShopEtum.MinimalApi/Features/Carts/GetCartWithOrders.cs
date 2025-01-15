using Microsoft.EntityFrameworkCore;
using ShopEtum.MinimalApi.Shared.Persistence;
using ShopEtum.MinimalApi.Shared.Slices;

namespace ShopEtum.MinimalApi.Features.Carts;

public class GetCartWithOrders : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/carts/{cartId}",
            async (int cartId, ShopEtumDbContext db, CancellationToken cancellationToken) =>
            {
                var cart = await db.Carts
                    .Include(c => c.Orders)
                    .ThenInclude(o => o.Product)
                    .FirstOrDefaultAsync(c => c.Id == cartId, cancellationToken);

                if (cart == null)
                {
                    return Results.NotFound("Cart not found");
                }

                var cartWithOrdersAndProducts = new
                {
                    cart.Id,
                    cart.CustomerIdentifier,
                    cart.CreatedDate,
                    cart.ModifiedDate,
                    Orders = cart.Orders.Select(o => new
                    {
                        o.Id,
                        o.ProductId,
                        o.Quantity,
                        o.CartId,
                        Product = new
                        {
                            o.Product.Id,
                            o.Product.Name,
                            o.Product.Description,
                            o.Product.Price
                        }
                    }).ToList()
                };

                return Results.Ok(cartWithOrdersAndProducts);
            }).WithName("GetCartWithOrders");
    }
}
