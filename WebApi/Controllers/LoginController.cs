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
            string? token = await service.LoginAsync(request);

            if(token is null)
                return NotFound("Usuário ou senha incorretos!");

            LoginResponse response = new()
            {
                Token = token
            };

            return Ok(response);
        }
    }
}
