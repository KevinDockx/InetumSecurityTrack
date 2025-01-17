namespace ShopEtum.MvcWebClient.Models.Dto;

public class OrderDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int CartId { get; set; }
    public ProductDto Product { get; set; }
}