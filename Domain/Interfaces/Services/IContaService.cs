using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IContaService
    {
        Task<AtualizarLimiteCreditoResponse> AtualizarLimiteCreditoAsync(AtualizarLimiteCreditoRequest request); 
        Task<CriarContaResponse> CriarContaAsync(CriarContaRequest request);
        Task<AtualizaReservaResponse> AtualizarReservaAsync(AtualizaValorContaRequest request);
        Task<AtualizaSaldoResponse> AtualizarSaldoAsync(AtualizaValorContaRequest request);
        Task<AtualizaSaldoCreditoResponse> AtualizarSaldoCreditoAsync(AtualizaValorContaRequest request);
        Task<List<Conta>> BuscarContaPorClienteAsync(uint idCliente);
        Task<DeletarContaResponse?> DeletarContaAsync(DeletarContaRequest request);
        Task<TransferenciaResponse> ProcessarTransferenciaAsync(TransferenciaRequest request);
    }
}
