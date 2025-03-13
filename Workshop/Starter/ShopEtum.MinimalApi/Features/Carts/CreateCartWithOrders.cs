using ShopEtum.MinimalApi.Shared.Slices;
using ShopEtum.MinimalApi.Shared.Domain.Entities;
using ShopEtum.MinimalApi.Shared.Persistence;
using ShopEtum.MinimalApi.Shared.Security;

namespace ShopEtum.MinimalApi.Features.Carts;

public class CreateCartWithOrders : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/carts/with-orders",
            async (CartWithOrdersForCreationDto cartDto,
                ShopEtumDbContext db,
                ICurrentUserService currentUserService,
                CancellationToken cancellationToken) =>
            {
                // demo code!  IRL, include an additional validation step
                var cart = new Cart(currentUserService.UserId);

                foreach (var orderDto in cartDto.Orders)
                {
                    var product = await db.Products.FindAsync([orderDto.ProductId],
                        cancellationToken: cancellationToken);
                    if (product == null)
                    {
                        return Results.NotFound($"Product with ID {orderDto.ProductId} not found");
                    }

                    var order = new Order(orderDto.ProductId, orderDto.Quantity)
                    {
                        CartId = cart.Id
                    };

                    cart.Orders.Add(order);
                }

                db.Carts.Add(cart);
                await db.SaveChangesAsync(cancellationToken);

                var createdCart = new
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
                        o.CartId
                    }).ToList()
                };

                return Results.Created($"/carts/{cart.Id}", createdCart);
            }).WithName("CreateCartWithOrders");
    }


    public class CartWithOrdersForCreationDto
    {
        public List<OrderForCreationDto> Orders { get; set; } = [];
    }

    public class OrderForCreationDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
