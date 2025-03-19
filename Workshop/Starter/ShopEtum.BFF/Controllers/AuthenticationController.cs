using Microsoft.AspNetCore.Mvc;

namespace ShopEtum.BFF.Controllers;

public class AuthenticationController(ILogger<AuthenticationController> logger) : Controller
{
    private readonly ILogger<AuthenticationController> _logger = logger;

} 