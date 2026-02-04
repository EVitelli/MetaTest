using Domain.Entities;
using Domain.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;

namespace Domain.Test.ServicesTest.UsuarioServiceTest
{
    public class CriarUsuarioAsyncTest : BaseUsuarioServiceTest
    {
        [Fact]
        public async Task CriarUsuarioAsync_ComRequestNulo_DeveLancarArgumentNullExceptionAsync()
        {
            //Arrange
            CriarUsuarioRequest? request = null;

            //Act
            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.CriarUsuarioAsync(request);
            });

            //Assert
            result
                .ShouldNotBeNull()
                .ShouldBeOfType<ArgumentNullException>();

            result.Message
                .ShouldBe("Value cannot be null. (Parameter 'request')");
        }

        [Fact]
        public async Task CriarUsuarioAsync_ComRequestValido_DeveRetornarCriarUsuarioResponseAsync()
        {
            //Arrange
            var request = new CriarUsuarioRequest
            {
                Nome = "Usuario Teste",
                Tipo = Enums.ETipoUsuario.Administrador,
                Cpf = "12345678900",
                Email = "teste@email.com",
                Senha = "SenhaForte123!",
                Operador = "Sistema"
            };

            Usuario usuarioDb = Substitute.For<Usuario>();
            usuarioDb.Id = 1;
            usuarioDb.Email = "teste@email.com";
            usuarioDb.CriadoEm = new DateTime(2026, 01, 01, 10, 22, 54);

            repository.CriarUsuarioAsync(Arg.Any<Usuario>()).ReturnsForAnyArgs(usuarioDb);

            //Act
            var result = await service.CriarUsuarioAsync(request);

            result.ShouldNotBeNull().ShouldBeOfType<CriarUsuarioResponse>();

            Convert.ToInt64(result.Id).ShouldBe(1);
            result.Email.ShouldBe("teste@email.com");
            result.DataCriacao.ShouldBe(new DateTime(2026, 01, 01, 10, 22, 54));
        }

        [Fact]
        public async Task CriarUsuarioAsync_QuandoRepositoryLancarArgumentNullException_DeveLancarArgumentExceptionAsync()
        {
            //Arrange
            var request = new CriarUsuarioRequest
            {
                Nome = "Usuario Teste",
                Tipo = Enums.ETipoUsuario.Administrador,
                Cpf = "12345678900",
                Email = "teste@email.com",
                Senha = "SenhaForte123!",
                Operador = "Sistema"
            };

            Usuario? usuario = null;
            repository.CriarUsuarioAsync(usuario).ThrowsAsyncForAnyArgs<ArgumentNullException>();

            //Act
            var result = await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.CriarUsuarioAsync(request);
            });

            result.ShouldNotBeNull().ShouldBeOfType<ArgumentException>();
            result.Message.ShouldBe("Erro ao criar um novo usuario.");
            result.InnerException.ShouldNotBeNull().ShouldBeOfType<ArgumentNullException>();
        }


        [Fact]
        public async Task CriarUsuarioAsync_QuandoRepositoryLancarQualquerException_DeveLancarExceptionAsync()
        {
            //Arrange
            var request = new CriarUsuarioRequest
            {
                Nome = "Usuario Teste",
                Tipo = Enums.ETipoUsuario.Administrador,
                Cpf = "12345678900",
                Email = "teste@email.com",
                Senha = "SenhaForte123!",
                Operador = "Sistema"
            };

            repository.CriarUsuarioAsync(Arg.Any<Usuario>()).ThrowsAsyncForAnyArgs<Exception>();

            //Act
            var result = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await service.CriarUsuarioAsync(request);
            });

            result.ShouldNotBeNull().ShouldBeOfType<Exception>();
            result.Message.ShouldBe("Erro inesperado ao criar um novo usuario.");
            result.InnerException.ShouldNotBeNull().ShouldBeOfType<Exception>();
        }
    }
}
