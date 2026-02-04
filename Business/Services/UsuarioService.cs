using Domain.Auth;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace Business.Services
{
    public class UsuarioService(IUsuarioRepository repository, IContaService contaService) : IUsuarioService
    {
        public async Task<CriarUsuarioResponse> CriarUsuarioAsync(CriarUsuarioRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            try
            {
                var hash = PasswordHasher.HashPassword(request.Senha, out byte[] salt);

                Usuario usuarioCriado = await repository.CriarUsuarioAsync(new Usuario
                {
                    Nome = request.Nome,
                    Tipo = request.Tipo,
                    Cpf = request.Cpf,
                    Email = request.Email,
                    Status = EStatus.Ativo,
                    Hash = hash,
                    Salt = salt,
                    AtualizadoPor = request.Operador,
                });

                return new CriarUsuarioResponse
                {
                    Id = usuarioCriado.Id,
                    Email = usuarioCriado.Email,
                    DataCriacao = usuarioCriado.CriadoEm
                };
            }
            catch (ArgumentNullException ae)
            {
                throw new ArgumentException(message: "Erro ao criar um novo usuario.", ae);
            }
            catch (Exception e)
            {
                throw new Exception(message: "Erro inesperado ao criar um novo usuario.", e);
            }
        }

        public async Task<UsuarioResponse?> BuscarUsuarioAsync(uint id)
        {
            var usuario = await repository.BuscarUsuarioAsync(id);

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

        public async Task<UsuarioAuthInfoResponse?> BuscarAuthInfoAsync(string email)
        {
            Usuario? usuario = await repository.BuscarUsuarioPorEmailAsync(email);

            if (usuario is null)
                return null;

            return new UsuarioAuthInfoResponse
            {
                Id = usuario.Id,
                Tipo = usuario.Tipo,
                Email = usuario.Email,
                Hash = usuario.Hash,
                Salt = usuario.Salt
            };
        }

        public async Task<DeletarUsuarioResponse?> DeletarUsuarioAsync(DeletarUsuarioRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            try
            {
                Usuario? usuario = await repository.BuscarUsuarioAsync(request.Id);

                if (usuario is null || usuario.Status == EStatus.Inativo)
                    return null;

                List<Domain.Models.Conta> contas = await contaService.BuscarContaPorClienteAsync(usuario.Id);

                foreach (var conta in contas)
                {
                    await contaService.DeletarContaAsync(new()
                    {
                        IdConta = conta.Id,
                        Operador = request.Operador

                    });
                }

                usuario.AtualizadoPor = request.Operador;

                usuario = await repository.DeletarUsuarioAsync(usuario);

                return new DeletarUsuarioResponse
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    DataDelecao = usuario.DeletadoEm!.Value
                };
            }
            catch (InvalidOperationException IOEx)
            {
                throw new Exception("Não foi possível remover usuário." , IOEx);
            }
            catch(Exception ex)
            {
                throw new Exception("Erro inesperado ao remover usuário.", ex);
            }

        }

    }
}
