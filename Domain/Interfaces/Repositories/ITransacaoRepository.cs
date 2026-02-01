using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ITransacaoRepository
    {
        Task<Transacao> AtualizarTransacaoAsync(Transacao transacao);
        Task<Transacao> AtualizarTransacaoAsync(Transacao transacao, Transferencia transferencia);
        Task<Transacao> CriarTransacaoAsync(Transacao transacao);
    }
}
