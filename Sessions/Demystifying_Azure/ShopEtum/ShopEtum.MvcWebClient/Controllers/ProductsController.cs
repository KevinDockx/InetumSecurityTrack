using Microsoft.AspNetCore.Mvc;
using ShopEtum.MvcWebClient.Models.Dto;

namespace ShopEtum.MvcWebClient.Controllers;

public class ProductsController(IHttpClientFactory httpClientFactory) : Controller
{ 
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<IActionResult> Index()
    {
        var httpClient = _httpClientFactory.CreateClient("ApiClient");
        var products = await httpClient.GetFromJsonAsync<List<ProductDto>>("/products");
        return View(products);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder(int productId, int quantity)
    {
        var httpClient = _httpClientFactory.CreateClient("ApiClient");
        string cartId = Request.Cookies["CartId"] ?? string.Empty;

        if (string.IsNullOrEmpty(cartId))
        {
            var response = await httpClient.PostAsJsonAsync("/carts/with-orders",
                new CartWithOrdersForCreationDto
                {
                    Orders = new List<OrderForCreationDto> { new() { ProductId = productId, Quantity = quantity } }
                });

            if (response.IsSuccessStatusCode)
            {
                var createdCart = await response.Content.ReadFromJsonAsync<CartDto>()
                    ?? throw new ArgumentNullException("Cart not found in response.");
                Response.Cookies.Append("CartId", createdCart.Id.ToString(), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });
            }
            else
            {
                throw new Exception("Something went wrong. All is fine. This is just a demo.");
            }
        }
        else
        {
            var cart = await httpClient.GetFromJsonAsync<CartDto>($"/carts/{cartId}");

            var existingOrder = cart?.Orders.FirstOrDefault(o => o.ProductId == productId);
            if (existingOrder != null)
            {
                existingOrder.Quantity += quantity;
                var response = await httpClient.PutAsJsonAsync($"/orders/{existingOrder.Id}", existingOrder);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Something went wrong. All is fine. This is just a demo.");
                }
            }
            else
            {
                var response = await httpClient.PostAsJsonAsync($"/carts/{cartId}/orders", 
                    new OrderForCreationDto { ProductId = productId, Quantity = quantity });

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Something went wrong. All is fine. This is just a demo.");
                }
            }
        }

        return RedirectToAction("Index", "Cart");
    }
}
