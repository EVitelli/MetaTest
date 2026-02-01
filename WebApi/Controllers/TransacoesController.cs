using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TransacoesController(ITransacaoService service): ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ProcessarTransacao(TransacaoRequest transacao)
        {
            return Ok(await service.ProcessarTransacaoAsync(transacao));
        }
    }
}
