using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository(DatabaseContext context) : IUsuarioRepository
    {
        public Usuario? GetUsuario(int id)
        {
            return context.Usuarios.Find(id);
        }
    }
}
