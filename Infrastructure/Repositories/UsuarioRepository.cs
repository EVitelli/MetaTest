using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository(DatabaseContext context) : IUsuarioRepository
    {
        //TODO: Remove if don't use
        public async Task<Usuario?> AtualizaUsuario(Usuario usuario)
        {
            Usuario? usuarioDb = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == usuario.Id);

            if (usuarioDb is null)
                return null;

            usuarioDb.Status = usuario.Status;
            usuarioDb.Nome = usuario.Nome;
            usuarioDb.Tipo = usuario.Tipo;
            usuarioDb.Cpf = usuario.Cpf;
            usuarioDb.Email = usuario.Email;
            usuarioDb.Hash = usuario.Hash;
            usuarioDb.AtualizadoEm = DateTime.Now;

            await context.SaveChangesAsync();

            return usuarioDb;
        }

        public async Task<Usuario> CriarUsuarioAsync(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario?> DeletarUsuario(uint id)
        {
            Usuario? usuario = await this.GetUsuarioAsync(id);

            if (usuario is null)
                return null;

            if (usuario.Status == EStatus.Inativo)
                return usuario;

            usuario.Status = EStatus.Inativo;
            usuario.DeletadoEm = DateTime.Now;

            await context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario?> GetUsuarioAsync(uint id)
        {
            return await context.Usuarios.FindAsync(id);
        }
    }
}
