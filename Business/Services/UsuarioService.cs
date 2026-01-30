using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace Business.Services
{
    public class UsuarioService(IUsuarioRepository repository) : IUsuarioService
    {
        public UsuarioResponse? GetUsuario(int id)
        {
            var usuario = repository.GetUsuario(id);

            if (usuario is null)
                return null;
            
            return new UsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Cpf = usuario.Cpf,
                Email = usuario.Email,
                DataCriacao = usuario.CriadoEm
            };
        }
    }
}
