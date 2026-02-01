using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface ITransacaoService
    {
        public Task<bool> ProcessarTransacaoAsync(TransacaoRequest transacao);
    }
}
