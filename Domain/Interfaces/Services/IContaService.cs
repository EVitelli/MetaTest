using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IContaService
    {
        Task<PostContaResponse> CriarContaAsync(ContaRequest conta);
        Task<AtualizaLimiteCreditoResponse> AtualizarLimiteCreditoAsync(ContaRequest conta);
        Task<AtualizaReservaResponse> AtualizarReservaAsync(AtualizaValorContaRequest request);
        Task<AtualizaSaldoResponse> AtualizarSaldoAsync(AtualizaValorContaRequest request);
        Task<AtualizaSaldoCreditoResponse> AtualizarSaldoCreditoAsync(AtualizaValorContaRequest request);
        Task<GetContaResponse?> BuscarContaPorCodigoAsync(string codigoConta);
        Task<DeleteContaResponse> DeletarContaAsync(uint id);
        Task<TransferenciaResponse> ProcessarTransferenciaAsync(TransferenciaRequest request);
    }
}
