using Microsoft.EntityFrameworkCore;
using ShopEtum.MinimalApi.Shared.Slices;
using ShopEtum.MinimalApi.Shared.Domain.Entities;
using ShopEtum.MinimalApi.Shared.Persistence;

namespace ShopEtum.MinimalApi.Features.Orders;

public class UpdateOrder : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPut("/orders/{orderId}",
            async (int orderId,
                OrderForUpdateDto orderDto,
                ShopEtumDbContext db,
                CancellationToken cancellationToken) =>
            {
                // note: demo code!  IRL, include a validation step! 

                var order = await db.Orders
                    .Include(o => o.Product)
                    .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
                if (order == null)
                {
                    return Results.NotFound("Order not found");
                }

                order.Quantity = orderDto.Quantity;

                await db.SaveChangesAsync(cancellationToken);

                var updatedOrder = new
                {
                    order.Id,
                    order.ProductId,
                    order.Quantity,
                    order.CartId
                };

                return Results.Ok(updatedOrder);
            }).WithName("UpdateOrder");
    }

    public class OrderForUpdateDto
    { 
        public int Quantity { get; set; }
    }
}

