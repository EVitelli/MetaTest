using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Test.ServicesTest.UsuarioServiceTest
{
    public class BuscarUsuarioAsyncTest : BaseUsuarioServiceTest
    {
        [Fact]
        public async Task BuscarUsuarioAsync_DeveRetornarUsuarioQuandoExistir()
        {
            // Arrange
            Usuario usuario = Substitute.For<Usuario>();
            usuario.Id = 1;
            usuario.Status = EStatus.Ativo;
            usuario.Nome = "Nome Teste";
            usuario.Tipo = ETipoUsuario.Administrador;
            usuario.Email = "test@mail.com";
            usuario.Cpf = "12345678900";
            usuario.CriadoEm = DateTime.Now;
            usuario.AtualizadoEm = DateTime.Now;
            usuario.DeletadoEm = null;

            repository.BuscarUsuarioAsync(usuario.Id).Returns(usuario);

            //Act
            var result = await service.BuscarUsuarioAsync(usuario.Id);

            result.ShouldNotBeNull().ShouldBeOfType<UsuarioResponse>();

            result.Id.ShouldBe(usuario.Id);
            result.Status.ShouldBe(usuario.Status);
            result.Nome.ShouldBe(usuario.Nome);
            result.Tipo.ShouldBe(usuario.Tipo);
            result.Email.ShouldBe(usuario.Email);
            result.Cpf.ShouldBe(usuario.Cpf);
            result.DataCriacao.ShouldBe(usuario.CriadoEm);
            result.DataAtualizacao.ShouldBe(usuario.AtualizadoEm);
            result.DataDelecao.ShouldBeNull();
        }

        [Fact]
        public async Task BuscarUsuarioAsync_QuandoUsuarioNaoExistir_DeveRetornarNulo()
        {
            // Arrange
            uint usuarioId = 1;

            repository.BuscarUsuarioAsync(usuarioId).ReturnsNull();

            //Act
            var result = await service.BuscarUsuarioAsync(usuarioId);

            result.ShouldBeNull();
        }
    }
}
