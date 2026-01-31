using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace Business.Services
{
    public class UsuarioService(IUsuarioRepository repository) : IUsuarioService
    {
        public async Task<PostUsuarioResponse> CriarUsuarioAsync(UsuarioRequest usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            //TODO: add fluent validation

            Usuario usuarioCriado = await repository.CriarUsuarioAsync(new Usuario
            {
                Nome = usuario.Nome,
                Tipo = usuario.Tipo,
                Cpf = usuario.Cpf,
                Email = usuario.Email,
                Status = EStatus.Ativo
            });

            return new PostUsuarioResponse
            {
                Id = usuarioCriado.Id,
                Email = usuarioCriado.Email,
                DataCriacao = usuarioCriado.CriadoEm
            };
        }

        public async Task<GetUsuarioResponse?> BuscarUsuarioAsync(uint id)
        {
            var usuario = await repository.GetUsuarioAsync(id);

            if (usuario is null)
                return null;

            return new GetUsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Cpf = usuario.Cpf,
                Email = usuario.Email,
                DataCriacao = usuario.CriadoEm,
                DataAtualizacao = usuario.AtualizadoEm
            };
        }

        public async Task<UsuarioResponse?> BuscarTodasInfoUsuarioAsync(uint id)
        {
            var usuario = await repository.GetUsuarioAsync(id);

            if (usuario is null)
                return null;

            return new UsuarioResponse
            {
                Id = usuario.Id,
                Status = usuario.Status,
                Nome = usuario.Nome,
                Tipo = usuario.Tipo,
                Cpf = usuario.Cpf,
                Email = usuario.Email,
                DataCriacao = usuario.CriadoEm,
                DataAtualizacao = usuario.AtualizadoEm,
                DataDelecao = usuario.DeletadoEm
            };
        }

        public async Task<DeleteUsuarioResponse?> DeletarUsuarioAsync(uint id)
        {
            Usuario? usuario = await repository.DeletarUsuario(id);

            return usuario is null ? null : new DeleteUsuarioResponse
            {
                Id = usuario.Id,
                Email = usuario.Email,
                DataDelecao = usuario.DeletadoEm!.Value
            };
        }

    }
}
