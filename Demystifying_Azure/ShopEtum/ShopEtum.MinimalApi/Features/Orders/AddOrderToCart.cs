using Microsoft.EntityFrameworkCore;
using ShopEtum.MinimalApi.Shared.Domain.Entities;
using ShopEtum.MinimalApi.Shared.Persistence;
using ShopEtum.MinimalApi.Shared.Slices;

namespace ShopEtum.MinimalApi.Features.Orders;

public class AddOrderToCart : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/carts/{cartId}/orders",
            async (int cartId, 
                OrderForCreationDto orderDto, 
                ShopEtumDbContext db, 
                CancellationToken cancellationToken) =>
            {
                // note: demo code!  IRL, include a validation step! 

                var cart = await db.Carts
                    .Include(c => c.Orders)
                    .FirstOrDefaultAsync(c => c.Id == cartId, cancellationToken);
                if (cart == null)
                {
                    return Results.NotFound("Cart not found");
                }

                var product = await db.Products.FindAsync([orderDto.ProductId], cancellationToken);
                if (product == null)
                {
                    return Results.NotFound("Product not found");
                }

                var order = new Order(orderDto.ProductId, orderDto.Quantity)
                { 
                    CartId = cartId
                };

                cart.Orders.Add(order);
                await db.SaveChangesAsync(cancellationToken);

                var createdOrder = new
                {
                    order.Id,
                    order.ProductId,
                    order.Quantity,
                    order.CartId
                };

                return Results.Created($"/carts/{cartId}/orders/{order.Id}", createdOrder);
            }).WithName("AddOrderToCart");
    }

    public class OrderForCreationDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
