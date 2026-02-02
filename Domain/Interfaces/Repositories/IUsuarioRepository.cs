using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> AtualizarUsuarioAsync(Usuario usuario);
        Task<Usuario?> BuscarAuthInfoAsync(Usuario usuario);
        Task<Usuario?> BuscarUsuarioAsync(uint id);
        Task<Usuario> CriarUsuarioAsync(Usuario usuario);
        Task<Usuario?> DeletarUsuarioAsync(uint id);
    }
}
