namespace ShopEtum.MvcWebClient.Models.Dto;

public class CartDto
{
    public int Id { get; set; }
    public string CustomerIdentifier { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public List<OrderDto> Orders { get; set; } = new();
}