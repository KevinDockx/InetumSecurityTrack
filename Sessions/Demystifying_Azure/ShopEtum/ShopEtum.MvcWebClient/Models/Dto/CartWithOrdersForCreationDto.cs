namespace ShopEtum.MvcWebClient.Models.Dto;

public class CartWithOrdersForCreationDto
{
    public List<OrderForCreationDto> Orders { get; set; } = [];
}