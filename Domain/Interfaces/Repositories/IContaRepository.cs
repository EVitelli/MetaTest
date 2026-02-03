using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface 
        IContaRepository
    {
        Task<Conta> AtualizarContaAsync(Conta conta);
        Task<List<Conta>> BuscarContaPorClienteAsync(uint idCliente);
        Task<Conta?> BuscarContaPorCodigoAsync(string codigoConta);
        Task<Conta?> BuscarContaPorIdAsync(uint id);
        Task<Conta> CriarContaAsync(Conta conta);
        Task<Conta> DeletarContaAsync(Conta conta);
    }
}
