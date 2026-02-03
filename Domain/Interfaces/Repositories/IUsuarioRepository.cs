using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> AtualizarUsuarioAsync(Usuario usuario);
        Task<Usuario?> BuscarUsuarioPorEmailAsync(string email);
        Task<Usuario?> BuscarUsuarioAsync(uint id);
        Task<Usuario> CriarUsuarioAsync(Usuario usuario);
        Task<Usuario?> DeletarUsuarioAsync(Usuario usuario);
    }
}
