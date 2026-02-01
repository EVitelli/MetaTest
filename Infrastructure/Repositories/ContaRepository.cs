using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ContaRepository(DatabaseContext context) : IContaRepository
    {
        public async Task<Conta> AtualizarContaAsync(Conta conta)
        {
            ArgumentNullException.ThrowIfNull(conta);

            context.Contas.Update(conta);
            await context.SaveChangesAsync();

            return conta;
        }

        public async Task<Conta?> BuscarContaPorCodigoAsync(string codigoConta)
        {
            return await context.Contas.FirstOrDefaultAsync(x => x.Codigo == codigoConta );
        }

        public async Task<Conta> CriarContaAsync(Conta conta)
        {
            ArgumentNullException.ThrowIfNull(conta);

            await context.Contas.AddAsync(conta);
            await context.SaveChangesAsync();

            return conta;
        }
    }
}
