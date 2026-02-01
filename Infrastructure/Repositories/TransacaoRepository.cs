using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class TransacaoRepository(DatabaseContext context) : ITransacaoRepository
    {
        public async Task<Transacao> AtualizarTransacaoAsync(Transacao transacao)
        {
            ArgumentNullException.ThrowIfNull(transacao);

            transacao.AtualizadoEm = DateTime.Now;

            context.Transacoes.Update(transacao);
            await context.SaveChangesAsync();

            return transacao;
        }

        public async Task<Transacao> AtualizarTransacaoAsync(Transacao transacao, Transferencia transferencia)
        {
            ArgumentNullException.ThrowIfNull(transacao);
            ArgumentNullException.ThrowIfNull(transferencia);

            await context.Transferencias.AddAsync(transferencia);
            await context.SaveChangesAsync();

            await AtualizarTransacaoAsync(transacao);
            return transacao;
        }

        public async Task<Transacao> CriarTransacaoAsync(Transacao transacao)
        {
            ArgumentNullException.ThrowIfNull(transacao);

            transacao.CriadoEm = DateTime.Now;
            transacao.AtualizadoEm = DateTime.Now;

            await context.Transacoes.AddAsync(transacao);
            await context.SaveChangesAsync();

            return transacao;
        }
    }
}
