using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<GetUsuarioResponse?> BuscarUsuarioAsync(uint id);
        Task<UsuarioResponse?> BuscarTodasInfoUsuarioAsync(uint id);
        Task<UsuarioAuthInfoResponse?> BuscarAuthInfoAsync(string email);
        Task<CriarUsuarioResponse> CriarUsuarioAsync(CriarUsuarioRequest request);
        Task<DeletarUsuarioResponse?> DeletarUsuarioAsync(DeletarUsuarioRequest request);
    }
}
