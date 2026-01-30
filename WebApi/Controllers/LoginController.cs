using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController: ControllerBase
    {
        [HttpPost]
        public IActionResult Login()
        {
            return Ok("Login successful");
        }
    }
}
