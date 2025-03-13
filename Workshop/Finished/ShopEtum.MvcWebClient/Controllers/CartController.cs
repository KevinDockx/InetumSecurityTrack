using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopEtum.MvcWebClient.Models.Dto;

namespace ShopEtum.MvcWebClient.Controllers;

[Authorize]
public class CartController(IHttpClientFactory httpClientFactory) : Controller
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<IActionResult> Index()
    {

        string cartId = Request.Cookies["CartId"] ?? string.Empty;

        if (string.IsNullOrEmpty(cartId))
        {
            return View(new CartDto());
        }

        var httpClient = _httpClientFactory.CreateClient("ApiClient");
        var cart = await httpClient.GetFromJsonAsync<CartDto>($"/carts/{cartId}");

        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> RemoveOrder(int orderId)
    {
        string cartId = Request.Cookies["CartId"] ?? string.Empty;

        if (string.IsNullOrEmpty(cartId))
        {
            return RedirectToAction("Index");
        }

        var httpClient = _httpClientFactory.CreateClient("ApiClient");
        var response = await httpClient.DeleteAsync($"/carts/{cartId}/orders/{orderId}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Something went wrong. All is fine. This is just a demo.");
        }

        return RedirectToAction("Index");
    }
}