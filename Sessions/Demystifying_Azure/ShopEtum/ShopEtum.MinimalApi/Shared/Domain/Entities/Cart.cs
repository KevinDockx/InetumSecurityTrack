namespace ShopEtum.MinimalApi.Shared.Domain.Entities;

public class Cart(string customerIdentifier) : AuditableEntity
{
    public Cart(string customerIdentifier, DateTime createdDate) : this(customerIdentifier)
    {
        CreatedDate = createdDate;
    }

    public int Id { get; set; }
    public List<Order> Orders { get; set; } = [];
    public string CustomerIdentifier { get; set; } = customerIdentifier;
}