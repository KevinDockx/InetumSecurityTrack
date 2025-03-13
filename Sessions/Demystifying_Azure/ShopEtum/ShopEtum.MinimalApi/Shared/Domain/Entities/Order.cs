namespace ShopEtum.MinimalApi.Shared.Domain.Entities;

public class Order( int productId, int quantity) : AuditableEntity
{
    public Order(int productId, int quantity, DateTime createdDate)
       : this(productId, quantity)
    {
        CreatedDate = createdDate;
    }

    public int Id { get; set; }
    public int ProductId { get; set; } = productId;
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; } = quantity;
    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;
}