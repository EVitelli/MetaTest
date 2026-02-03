using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContasController(IContaService service): ControllerBase
    {
        [Authorize(Roles = "Gerente")]
        [HttpPost]
        public async Task<IActionResult> CriarContaASync(CriarContaRequest request)
        {
            request.GerenteId = this.GetClaimIdValue(User); ;
            request.Operador = this.GetClaimEmailValue(User);

            CriarContaResponse response = await service.CriarContaAsync(request);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarContaAsync(uint id)
        {
            DeletarContaRequest request = new()
            {
                IdConta = id,
                Operador = this.GetClaimEmailValue(User)
            };

            DeletarContaResponse? response = await service.DeletarContaAsync(request);

            return Ok(response);
        }

        [Authorize(Roles = "Gerente")]
        [HttpPut("atualizarLimiteCredito")]
        public async Task<IActionResult> AtualizarLimiteContaAsync(AtualizarLimiteCreditoRequest request)
        {
            request.Operador = this.GetClaimEmailValue(User);

            AtualizarLimiteCreditoResponse response = await service.AtualizarLimiteCreditoAsync(request);

            return Ok(response);
        }
    }
}
