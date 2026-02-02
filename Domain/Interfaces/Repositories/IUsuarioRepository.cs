using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> AtualizarUsuarioAsync(Usuario usuario);
        Task<Usuario?> BuscarUsuarioPorEmailAsync(string email);
        Task<Usuario?> BuscarUsuarioAsync(uint id);
        Task<Usuario> CriarUsuarioAsync(Usuario usuario);
        Task<Usuario?> DeletarUsuarioAsync(uint id);
    }
}
