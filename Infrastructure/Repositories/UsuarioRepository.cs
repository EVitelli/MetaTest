using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository(DatabaseContext context) : IUsuarioRepository
    {
        //TODO: Remove if don't use
        public async Task<Usuario?> AtualizarUsuarioAsync(Usuario usuario)
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
            usuarioDb.AtualizadoPor = usuario.AtualizadoPor;

            await context.SaveChangesAsync();

            return usuarioDb;
        }

        public async Task<Usuario?> BuscarUsuarioAsync(uint id)
        {
            return await context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario?> BuscarUsuarioPorEmailAsync(string email)
        {
            return await context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Usuario> CriarUsuarioAsync(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            usuario.CriadoEm = DateTime.Now;
            usuario.AtualizadoEm = DateTime.Now;
            usuario.AtualizadoPor = usuario.AtualizadoPor;
            usuario.Status = EStatus.Ativo;

            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario?> DeletarUsuarioAsync(Usuario usuario)
        {
            Usuario? usuarioDb = await this.BuscarUsuarioAsync(usuario.Id);

            if (usuarioDb is null)
                return null;

            if (usuarioDb.Status == EStatus.Inativo)
                return usuarioDb;

            usuarioDb.Status = EStatus.Inativo;
            usuarioDb.AtualizadoEm = DateTime.Now;
            usuarioDb.DeletadoEm = DateTime.Now;
            usuarioDb.AtualizadoPor = usuario.AtualizadoPor;

            await context.SaveChangesAsync();

            return usuarioDb;
        }

    }
}
