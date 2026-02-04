using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;

namespace Domain.Test.ServicesTest.UsuarioServiceTest
{
    public class DeletarUsuarioAsyncTest : BaseUsuarioServiceTest
    {

        [Fact]
        public async Task DeletarUsuarioAsync_ComRequestNulo_DeveLancarArgumentNullExceptionAsync()
        {
            //Arrange
            DeletarUsuarioRequest? request = null;

            //Act
            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.DeletarUsuarioAsync(request);
            });

            //Assert
            result
                .ShouldNotBeNull()
                .ShouldBeOfType<ArgumentNullException>();

            result.Message
                .ShouldBe("Value cannot be null. (Parameter 'request')");
        }

        [Fact]
        public async Task DeletarUsuarioAsync_QuandoUsuarioNaoExistir_DeveRetornarNulo()
        {
            // Arrange
            DeletarUsuarioRequest? request = new()
            {
                Id = 1,
                Operador = "Sistema"
            };


            Usuario usuario = null;

            repository.BuscarUsuarioAsync(1).Returns(usuario);

            //Act
            var result = await service.DeletarUsuarioAsync(request);

            //Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task DeletarUsuarioAsync_QuandoUsuarioEstiverInativo_DeveRetornarNulo_()
        {
            // Arrange
            DeletarUsuarioRequest? request = new()
            {
                Id = 1,
                Operador = "Sistema"
            };

            Usuario usuario = Substitute.For<Usuario>();
            usuario.Id = 1;
            usuario.Status = EStatus.Inativo;

            repository.BuscarUsuarioAsync(1).Returns(usuario);

            //Act
            var result = await service.DeletarUsuarioAsync(request);

            result.ShouldBeNull();
        }

        [Fact]
        public async Task DeletarUsuarioAsync_QuandoAContaTiverSaldo_DeveLancarException()
        {
            // Arrange
            DeletarUsuarioRequest? request = new()
            {
                Id = 1,
                Operador = "Sistema"
            };

            Usuario usuario = Substitute.For<Usuario>();
            usuario.Id = 1;

            Models.Conta conta = Substitute.For<Models.Conta>();

            repository.BuscarUsuarioAsync(1).Returns(usuario);
            contaService.BuscarContaPorClienteAsync(usuario.Id).Returns([conta]);
            contaService.DeletarContaAsync(Arg.Any<DeletarContaRequest>()).ThrowsAsync<InvalidOperationException>();

            //Act
            var result = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await service.DeletarUsuarioAsync(request);
            });

            result.ShouldNotBeNull().ShouldBeOfType<Exception>();
            result.Message.ShouldBe("Não foi possível remover usuário.");
        }

        [Fact]
        public async Task DeletarUsuarioAsync_QuandoOcorrerException_DeveLancarException()
        {
            // Arrange
            DeletarUsuarioRequest? request = new()
            {
                Id = 1,
                Operador = "Sistema"
            };

            repository.BuscarUsuarioAsync(1).ThrowsAsync<Exception>();

            //Act
            var result = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await service.DeletarUsuarioAsync(request);
            });

            result.ShouldNotBeNull().ShouldBeOfType<Exception>();
            result.Message.ShouldBe("Erro inesperado ao remover usuário.");
        }

        [Fact]
        public async Task DeletarUsuarioAsync_QuandoEstiverTudoCorreto_DeveRetornarUsuario()
        {
            // Arrange
            DeletarUsuarioRequest? request = new()
            {
                Id = 1,
                Operador = "Sistema"
            };

            Usuario usuario = Substitute.For<Usuario>();
            usuario.Id = 1;
            usuario.Email = "test@mail.com";
            usuario.DeletadoEm = new DateTime(2026, 01, 01, 10, 22, 54);

            Models.Conta conta = Substitute.For<Models.Conta>();

            repository.BuscarUsuarioAsync(1).Returns(usuario);
            contaService.BuscarContaPorClienteAsync(usuario.Id).Returns([conta]);
            await contaService.DeletarContaAsync(Arg.Any<DeletarContaRequest>());
            repository.DeletarUsuarioAsync(usuario).Returns(usuario);

            //Act
            var result = await service.DeletarUsuarioAsync(request);

            result.ShouldNotBeNull().ShouldBeOfType<DeletarUsuarioResponse>();
            result.Id.ShouldBe((uint)1);
            result.Email.ShouldBe("test@mail.com");
            result.DataDelecao.ShouldBe(new DateTime(2026, 01, 01, 10, 22, 54));
        }
    }
}
