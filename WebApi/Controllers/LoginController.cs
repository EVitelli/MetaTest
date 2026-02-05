using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController(ILoginService service): ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            LoginResponse? response = await service.LoginAsync(request);

            if(response is null)
                return NotFound("Usuário ou senha incorretos!");

            return Ok(response);
        }
    }
}
