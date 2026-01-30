using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsuariosController(IUsuarioService service): ControllerBase
    {
        [HttpPost]
        public IActionResult Post(UsuarioRequest usuario)
        {
            UsuarioResponse response = new UsuarioResponse
            {
                Id = 1,
                DataCriacao = DateTime.Now
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            UsuarioResponse? response = service.GetUsuario(id);

            return Ok(response);
        }
    }
}
