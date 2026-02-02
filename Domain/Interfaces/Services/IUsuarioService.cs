using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<GetUsuarioResponse?> BuscarUsuarioAsync(uint id);
        Task<UsuarioResponse?> BuscarTodasInfoUsuarioAsync(uint id);
        Task<UsuarioAuthInfoResponse?> BuscarAuthInfoAsync(string email);
        Task<PostUsuarioResponse> CriarUsuarioAsync(UsuarioRequest usuario);
        Task<DeleteUsuarioResponse?> DeletarUsuarioAsync(uint id);
    }
}
