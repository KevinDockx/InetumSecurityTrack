using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BFFSample.ClientWithLocalAPI.Controllers;

[Route("classiclocalapi/hello")]
[ApiController]
[Authorize]
public class ClassicLocalApiController : Controller
{
    [HttpGet]
    public ActionResult<IEnumerable<Claim>> Hello()
    {
        return Ok(new
        {
            Message = "Hello from the classic local API!  It seems you are:",
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}
