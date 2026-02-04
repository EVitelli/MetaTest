using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioResponse?> BuscarUsuarioAsync(uint id);
        Task<UsuarioAuthInfoResponse?> BuscarAuthInfoAsync(string email);
        Task<CriarUsuarioResponse> CriarUsuarioAsync(CriarUsuarioRequest request);
        Task<DeletarUsuarioResponse?> DeletarUsuarioAsync(DeletarUsuarioRequest request);
    }
}
