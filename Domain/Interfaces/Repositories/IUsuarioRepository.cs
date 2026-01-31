using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> AtualizaUsuario(Usuario usuario);
        Task<Usuario> CriarUsuarioAsync(Usuario usuario);
        Task<Usuario?> DeletarUsuario(uint id);
        Task<Usuario?> GetUsuarioAsync(uint id);
    }
}
