using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContasController(IContaService service): ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CriarConta(ContaRequest conta)
        {
            PostContaResponse response = await service.CriarContaAsync(conta);

            return Ok(response);
        }
    }
}
