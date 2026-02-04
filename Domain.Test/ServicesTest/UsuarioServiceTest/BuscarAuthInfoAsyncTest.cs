using Domain.Auth;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;

namespace Domain.Test.ServicesTest.UsuarioServiceTest
{
    public class BuscarAuthInfoAsyncTest: BaseUsuarioServiceTest
    {
        [Fact]
        public async Task BuscarAuthInfoAsync_DeveRetornarUsuarioQuandoExistir()
        {
            // Arrange
            string hash = PasswordHasher.HashPassword("SenhaForte123!", out byte[] salt);

            Usuario usuario = Substitute.For<Usuario>();
            usuario.Id = 1;
            usuario.Email = "test@mail.com";
            usuario.Tipo = ETipoUsuario.Administrador;
            usuario.Hash = hash;
            usuario.Salt = salt;

            repository.BuscarUsuarioPorEmailAsync(usuario.Email).Returns(usuario);

            //Act
            var result = await service.BuscarAuthInfoAsync(usuario.Email);

            result.ShouldNotBeNull().ShouldBeOfType<UsuarioAuthInfoResponse>();

            result.Id.ShouldBe(usuario.Id);
            result.Tipo.ShouldBe(usuario.Tipo);
            result.Email.ShouldBe(usuario.Email);
            result.Hash.ShouldBe(hash);
            result.Salt.ShouldBe(salt);
        }

        [Fact]
        public async Task BuscarUsuarioAsync_QuandoUsuarioNaoExistir_DeveRetornarNulo()
        {
            // Arrange
            string email = "test@mail.com";

            repository.BuscarUsuarioPorEmailAsync(email).ReturnsNull();

            // Act
            var result = await service.BuscarAuthInfoAsync(email);

            // Assert
            result.ShouldBeNull();
        }
    }
}
