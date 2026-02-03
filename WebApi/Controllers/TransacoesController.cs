using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TransacoesController(ITransacaoService service): ControllerBase
    {
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public async Task<IActionResult> ProcessarTransacao(TransacaoRequest transacao)
        {
            return Ok(await service.ProcessarTransacaoAsync(transacao));
        }
    }
}
