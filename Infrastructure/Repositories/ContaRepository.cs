using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class ContaRepository(DatabaseContext context) : IContaRepository
    {
        public async Task<Conta> CriarConta(Conta conta)
        {
            ArgumentNullException.ThrowIfNull(conta);

            await context.Contas.AddAsync(conta);
            await context.SaveChangesAsync();

            return conta;
        }
    }
}
