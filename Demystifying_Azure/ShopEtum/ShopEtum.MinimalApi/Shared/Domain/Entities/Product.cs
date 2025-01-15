namespace ShopEtum.MinimalApi.Shared.Domain.Entities;


public class Product(string name, string? description, decimal price) : AuditableEntity
{
    public Product(string name, string? description, decimal price, DateTime createdDate) 
        : this(name, description, price)
    {
        CreatedDate = createdDate;      
    }

    public int Id { get; set; }
    public string Name { get; set; } = name;
    public string? Description { get; set; } = description;
    public decimal Price { get; set; } = price;
}