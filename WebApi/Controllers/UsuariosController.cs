using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsuariosController(IUsuarioService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync(UsuarioRequest usuario)
        {
            PostUsuarioResponse response = await service.CriarUsuarioAsync(usuario);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(uint id)
        {
            GetUsuarioResponse? response = await service.BuscarUsuarioAsync(id);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(uint id)
        {
            DeleteUsuarioResponse? response = await service.DeletarUsuarioAsync(id);

            return Ok(response);
        }
    }
}
