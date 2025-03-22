using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController: ControllerBase
{
    [HttpGet("[action]")]
    public bool Login()
    {
        return true;
    }
}