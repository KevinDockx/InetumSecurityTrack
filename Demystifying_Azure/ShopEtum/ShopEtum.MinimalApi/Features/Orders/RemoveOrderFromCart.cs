using Microsoft.EntityFrameworkCore;
using ShopEtum.MinimalApi.Shared.Slices;
using ShopEtum.MinimalApi.Shared.Domain.Entities;
using ShopEtum.MinimalApi.Shared.Persistence;
using static ShopEtum.MinimalApi.Features.Products.CreateProduct;

namespace ShopEtum.MinimalApi.Features.Orders;

public class RemoveOrderFromCart : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapDelete("/carts/{cartId}/orders/{orderId}", 
            async (int cartId, 
                int orderId, 
                ShopEtumDbContext db, 
                CancellationToken cancellationToken) =>
        {
            var cart = await db.Carts
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == cartId, cancellationToken);
            if (cart == null)
            {
                return Results.NotFound("Cart not found");
            }

            var order = cart.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return Results.NotFound("Order not found in the cart");
            }

            cart.Orders.Remove(order);
            await db.SaveChangesAsync(cancellationToken);

            return Results.NoContent();
        }).WithName("RemoveOrderFromCart");
    }

}
