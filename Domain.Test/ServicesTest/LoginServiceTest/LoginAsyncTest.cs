using Domain.Auth;
using Domain.Enums;
using Domain.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;

namespace Domain.Test.ServicesTest.LoginServiceTest
{
    public class LoginAsyncTest : BaseLoginServiceTest
    {
        [Fact]
        public async Task LoginAsync_ComCredenciaisValidas_DeveRetornarToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "usuario@example.com",
                Senha = "senha123"
            };

            // Gerar hash e salt para a senha
            var hash = PasswordHasher.HashPassword("senha123", out var salt);

            var usuarioAuthInfo = new UsuarioAuthInfoResponse
            {
                Id = 1,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente,
                Hash = hash,
                Salt = salt
            };

            var tokenResponse = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";

            usuarioService.BuscarAuthInfoAsync("usuario@example.com").Returns(usuarioAuthInfo);
            authService.GenerateToken(Arg.Any<TokenRequest>()).Returns(tokenResponse);

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            response.ShouldNotBeNull().ShouldBeOfType<LoginResponse>();
            response.Token.ShouldBe(tokenResponse);
            await usuarioService.Received(1).BuscarAuthInfoAsync("usuario@example.com");
            authService.Received(1).GenerateToken(Arg.Any<TokenRequest>());
        }

        [Fact]
        public async Task LoginAsync_ComEmailInvalido_DeveRetornarNull()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "naoexiste@example.com",
                Senha = "senha123"
            };

            usuarioService.BuscarAuthInfoAsync("naoexiste@example.com").Returns((UsuarioAuthInfoResponse?)null);

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            response.ShouldBeNull();
            await usuarioService.Received(1).BuscarAuthInfoAsync("naoexiste@example.com");
            authService.DidNotReceive().GenerateToken(Arg.Any<TokenRequest>());
        }

        [Fact]
        public async Task LoginAsync_ComSenhaInvalida_DeveRetornarNull()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "usuario@example.com",
                Senha = "senhaErrada"
            };

            // Gerar hash e salt para a senha correta
            var hash = PasswordHasher.HashPassword("senha123", out var salt);

            var usuarioAuthInfo = new UsuarioAuthInfoResponse
            {
                Id = 1,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente,
                Hash = hash,
                Salt = salt
            };

            usuarioService.BuscarAuthInfoAsync("usuario@example.com").Returns(usuarioAuthInfo);

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            response.ShouldBeNull();
            await usuarioService.Received(1).BuscarAuthInfoAsync("usuario@example.com");
            authService.DidNotReceive().GenerateToken(Arg.Any<TokenRequest>());
        }

        [Fact]
        public async Task LoginAsync_ComTipoUsuarioCliente_DeveRetornarToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "cliente@example.com",
                Senha = "senha123"
            };

            var hash = PasswordHasher.HashPassword("senha123", out var salt);

            var usuarioAuthInfo = new UsuarioAuthInfoResponse
            {
                Id = 5,
                Email = "cliente@example.com",
                Tipo = ETipoUsuario.Cliente,
                Hash = hash,
                Salt = salt
            };

            var tokenResponse = "token-cliente";

            usuarioService.BuscarAuthInfoAsync("cliente@example.com").Returns(usuarioAuthInfo);
            authService.GenerateToken(Arg.Any<TokenRequest>()).Returns(tokenResponse);

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            response.ShouldNotBeNull().ShouldBeOfType<LoginResponse>();
            response.Token.ShouldBe("token-cliente");
            authService.Received(1).GenerateToken(Arg.Is<TokenRequest>(t => 
                t.Id == 5 && 
                t.Email == "cliente@example.com" && 
                t.Tipo == ETipoUsuario.Cliente));
        }

        [Fact]
        public async Task LoginAsync_ComTipoUsuarioGerente_DeveRetornarToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "gerente@example.com",
                Senha = "senha123"
            };

            var hash = PasswordHasher.HashPassword("senha123", out var salt);

            var usuarioAuthInfo = new UsuarioAuthInfoResponse
            {
                Id = 10,
                Email = "gerente@example.com",
                Tipo = ETipoUsuario.Gerente,
                Hash = hash,
                Salt = salt
            };

            var tokenResponse = "token-gerente";

            usuarioService.BuscarAuthInfoAsync("gerente@example.com").Returns(usuarioAuthInfo);
            authService.GenerateToken(Arg.Any<TokenRequest>()).Returns(tokenResponse);

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            response.ShouldNotBeNull().ShouldBeOfType<LoginResponse>();
            response.Token.ShouldBe("token-gerente");
            authService.Received(1).GenerateToken(Arg.Is<TokenRequest>(t => 
                t.Id == 10 && 
                t.Email == "gerente@example.com" && 
                t.Tipo == ETipoUsuario.Gerente));
        }

        [Fact]
        public async Task LoginAsync_ComTipoUsuarioAdministrador_DeveRetornarToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "admin@example.com",
                Senha = "senha123"
            };

            var hash = PasswordHasher.HashPassword("senha123", out var salt);

            var usuarioAuthInfo = new UsuarioAuthInfoResponse
            {
                Id = 1,
                Email = "admin@example.com",
                Tipo = ETipoUsuario.Administrador,
                Hash = hash,
                Salt = salt
            };

            var tokenResponse = "token-admin";

            usuarioService.BuscarAuthInfoAsync("admin@example.com").Returns(usuarioAuthInfo);
            authService.GenerateToken(Arg.Any<TokenRequest>()).Returns(tokenResponse);

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            response.ShouldNotBeNull().ShouldBeOfType<LoginResponse>();
            response.Token.ShouldBe("token-admin");
            authService.Received(1).GenerateToken(Arg.Is<TokenRequest>(t => 
                t.Id == 1 && 
                t.Email == "admin@example.com" && 
                t.Tipo == ETipoUsuario.Administrador));
        }

        [Fact]
        public async Task LoginAsync_DevePassarCorretosDadosParaGerarToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "usuario@example.com",
                Senha = "senha123"
            };

            var hash = PasswordHasher.HashPassword("senha123", out var salt);

            var usuarioAuthInfo = new UsuarioAuthInfoResponse
            {
                Id = 42,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente,
                Hash = hash,
                Salt = salt
            };

            var tokenResponse = "token-valido";

            usuarioService.BuscarAuthInfoAsync("usuario@example.com").Returns(usuarioAuthInfo);
            authService.GenerateToken(Arg.Any<TokenRequest>()).Returns(tokenResponse);

            // Act
            await service.LoginAsync(request);

            // Assert
            authService.Received(1).GenerateToken(Arg.Is<TokenRequest>(t => 
                t.Id == 42 && 
                t.Email == "usuario@example.com"));
        }

        [Fact]
        public async Task LoginAsync_ComRepositoryError_DeveLancarException()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "usuario@example.com",
                Senha = "senha123"
            };

            usuarioService.BuscarAuthInfoAsync("usuario@example.com")
                .Throws(new Exception("Erro na base de dados"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(() => service.LoginAsync(request));
        }

        [Fact]
        public async Task LoginAsync_ComSenhaVazia_DeveRetornarNull()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "usuario@example.com",
                Senha = ""
            };

            var hash = PasswordHasher.HashPassword("senha123", out var salt);

            var usuarioAuthInfo = new UsuarioAuthInfoResponse
            {
                Id = 1,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente,
                Hash = hash,
                Salt = salt
            };

            usuarioService.BuscarAuthInfoAsync("usuario@example.com").Returns(usuarioAuthInfo);

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            response.ShouldBeNull();
        }

        [Fact]
        public async Task LoginAsync_ComEmailVazio_DeveRetornarNull()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "",
                Senha = "senha123"
            };

            usuarioService.BuscarAuthInfoAsync("").Returns((UsuarioAuthInfoResponse?)null);

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            response.ShouldBeNull();
        }

        [Fact]
        public async Task LoginAsync_ComEmailComEspacos_DeveVerificarEmail()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "usuario@example.com",
                Senha = "senha123"
            };

            var hash = PasswordHasher.HashPassword("senha123", out var salt);

            var usuarioAuthInfo = new UsuarioAuthInfoResponse
            {
                Id = 1,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente,
                Hash = hash,
                Salt = salt
            };

            var tokenResponse = "token-valido";

            usuarioService.BuscarAuthInfoAsync("usuario@example.com").Returns(usuarioAuthInfo);
            authService.GenerateToken(Arg.Any<TokenRequest>()).Returns(tokenResponse);

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            response.ShouldNotBeNull();
            await usuarioService.Received(1).BuscarAuthInfoAsync("usuario@example.com");
        }

        [Fact]
        public async Task LoginAsync_DeveValidarSenhaComSaltCorreto()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "usuario@example.com",
                Senha = "senha123"
            };

            var hash = PasswordHasher.HashPassword("senha123", out var salt);

            var usuarioAuthInfo = new UsuarioAuthInfoResponse
            {
                Id = 1,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente,
                Hash = hash,
                Salt = salt
            };

            var tokenResponse = "token-valido";

            usuarioService.BuscarAuthInfoAsync("usuario@example.com").Returns(usuarioAuthInfo);
            authService.GenerateToken(Arg.Any<TokenRequest>()).Returns(tokenResponse);

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            response.ShouldNotBeNull().ShouldBeOfType<LoginResponse>();
            response.Token.ShouldBe("token-valido");
        }

        [Fact]
        public async Task LoginAsync_QuandoGerarTokenFalha_DeveLancarException()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "usuario@example.com",
                Senha = "senha123"
            };

            var hash = PasswordHasher.HashPassword("senha123", out var salt);

            var usuarioAuthInfo = new UsuarioAuthInfoResponse
            {
                Id = 1,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente,
                Hash = hash,
                Salt = salt
            };

            usuarioService.BuscarAuthInfoAsync("usuario@example.com").Returns(usuarioAuthInfo);
            authService.GenerateToken(Arg.Any<TokenRequest>())
                .Throws(new Exception("Erro ao gerar token"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(() => service.LoginAsync(request));
        }

        [Fact]
        public async Task LoginAsync_ComMultiplasSenhasIguais_DeveRetornarToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "usuario@example.com",
                Senha = "senha123"
            };

            var hash = PasswordHasher.HashPassword("senha123", out var salt);

            var usuarioAuthInfo = new UsuarioAuthInfoResponse
            {
                Id = 1,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente,
                Hash = hash,
                Salt = salt
            };

            var tokenResponse = "token-valido";

            usuarioService.BuscarAuthInfoAsync("usuario@example.com").Returns(usuarioAuthInfo);
            authService.GenerateToken(Arg.Any<TokenRequest>()).Returns(tokenResponse);

            // Act
            var response = await service.LoginAsync(request);

            // Assert
            response.ShouldNotBeNull();
        }
    }
}
