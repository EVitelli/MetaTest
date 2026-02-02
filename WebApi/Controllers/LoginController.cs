using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
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
            var hasher = new PasswordHasher<LoginRequest>();
            usuario.Senha = hasher.HashPassword(usuario, usuario.Senha);

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
