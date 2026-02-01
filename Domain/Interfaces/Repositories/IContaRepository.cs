using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IContaRepository
    {
        Task<Conta> AtualizarContaAsync(Conta conta);
        Task<Conta?> BuscarContaPorCodigoAsync(string codigoConta);
        Task<Conta> CriarContaAsync(Conta conta);
    }
}
