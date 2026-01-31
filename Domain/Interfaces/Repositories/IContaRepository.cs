using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IContaRepository
    {
        Task<Conta> CriarConta(Conta conta);
    }
}
