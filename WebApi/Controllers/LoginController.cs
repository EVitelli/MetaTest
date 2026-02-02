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
        public async Task<IActionResult> LoginAsync(LoginRequest usuario)
        {
            string token = await service.LoginAsync(usuario);

            if(token is null)
                return NotFound("Usuário ou senha incorretos!");

            LoginResponse response = new LoginResponse
            {
                Token = token
            };

            return Ok(response);
        }
    }
}
