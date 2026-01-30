using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
       UsuarioResponse? GetUsuario(int id);
    }
}
