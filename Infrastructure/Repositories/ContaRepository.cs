using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ContaRepository(DatabaseContext context) : IContaRepository
    {
        public async Task<Conta> AtualizarContaAsync(Conta conta)
        {
            ArgumentNullException.ThrowIfNull(conta);

            conta.AtualizadoEm = DateTime.Now;

            context.Contas.Update(conta);
            await context.SaveChangesAsync();

            return conta;
        }

        public async Task<List<Conta>> BuscarContaPorClienteAsync(uint idCliente)
        {
            return await context.Contas.Where(x => x.IdUsuarioCliente == idCliente).ToListAsync();
        }

        public async Task<Conta?> BuscarContaPorCodigoAsync(string codigoConta)
        {
            return await context.Contas.FirstOrDefaultAsync(x => x.Codigo == codigoConta );
        }

        public async Task<Conta?> BuscarContaPorIdAsync(uint id)
        {
            return await context.Contas.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Conta> CriarContaAsync(Conta conta)
        {
            ArgumentNullException.ThrowIfNull(conta);

            await context.Contas.AddAsync(conta);
            await context.SaveChangesAsync();

            return conta;
        }

        public async Task<Conta?> DeletarContaAsync(Conta conta)
        {
            Conta? contaDb = await BuscarContaPorIdAsync(conta.Id);

            if (contaDb is null)
                return null;

            if (contaDb.Status == EStatus.Inativo)
                return contaDb;

            if (contaDb.Saldo > 0 || contaDb.Reservado > 0 || contaDb.SaldoCredito != contaDb.LimiteCredito)
            {
                throw new InvalidOperationException("Conta com saldo ou valores reservados. Não é possível deletar a conta.");
            }

            contaDb.Status = EStatus.Inativo;
            contaDb.LimiteCredito = 0;
            contaDb.SaldoCredito = 0;
            contaDb.AtualizadoPor = conta.AtualizadoPor;
            contaDb.AtualizadoEm = DateTime.Now;
            contaDb.DeletadoEm = DateTime.Now;

            await context.SaveChangesAsync();
            return contaDb;
        }
    }
}
