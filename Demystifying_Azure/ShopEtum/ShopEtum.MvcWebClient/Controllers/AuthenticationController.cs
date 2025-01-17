using Microsoft.AspNetCore.Mvc;

namespace ShopEtum.MvcWebClient.Controllers;

public class AuthenticationController : Controller
{
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(ILogger<AuthenticationController> logger)
    {
        _logger = logger;
    } 
     
}
