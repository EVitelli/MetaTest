using Domain.Enums;
using Domain.Models;
using Shouldly;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

namespace Domain.Test.ServicesTest.AuthServiceTest
{
    public class GenerateTokenTest : BaseAuthServiceTest
    {
        [Fact]
        public void GenerateToken_ComDadosValidos_DeveRetornarTokenValido()
        {
            // Arrange
            var request = new TokenRequest
            {
                Id = 1,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente
            };

            // Act
            var token = service.GenerateToken(request);

            // Assert
            token.ShouldNotBeNullOrEmpty();
            token.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GenerateToken_ComDadosValidos_DeveRetornarTokenJWT()
        {
            // Arrange
            var request = new TokenRequest
            {
                Id = 100,
                Email = "gerente@example.com",
                Tipo = ETipoUsuario.Gerente
            };

            // Act
            var token = service.GenerateToken(request);

            // Assert
            token.ShouldNotBeNullOrEmpty();
            // JWT tokens têm 3 partes separadas por pontos
            var parts = token.Split('.');
            parts.Length.ShouldBe(3);
        }

        [Fact]
        public void GenerateToken_DeveConterIdNoToken()
        {
            // Arrange
            var request = new TokenRequest
            {
                Id = 42,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente
            };

            // Act
            var token = service.GenerateToken(request);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Assert
            jwtToken.ShouldNotBeNull();
            var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id");
            idClaim.ShouldNotBeNull();
            idClaim.Value.ShouldBe("42");
        }

        [Fact]
        public void GenerateToken_DeveConterEmailNoToken()
        {
            // Arrange
            var request = new TokenRequest
            {
                Id = 1,
                Email = "teste@example.com",
                Tipo = ETipoUsuario.Cliente
            };

            // Act
            var token = service.GenerateToken(request);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Assert
            jwtToken.ShouldNotBeNull();
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email");
            emailClaim.ShouldNotBeNull();
            emailClaim.Value.ShouldBe("teste@example.com");
        }

        [Fact]
        public void GenerateToken_DeveConterRoleNoToken()
        {
            // Arrange
            var request = new TokenRequest
            {
                Id = 1,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Gerente
            };

            // Act
            var token = service.GenerateToken(request);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Assert
            jwtToken.ShouldNotBeNull();
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
            roleClaim.ShouldNotBeNull();
            roleClaim.Value.ShouldBe("Gerente");
        }

        [Fact]
        public void GenerateToken_ComTipoAdministrador_DeveConterRoleAdministrador()
        {
            // Arrange
            var request = new TokenRequest
            {
                Id = 1,
                Email = "admin@example.com",
                Tipo = ETipoUsuario.Administrador
            };

            // Act
            var token = service.GenerateToken(request);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Assert
            jwtToken.ShouldNotBeNull();
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
            roleClaim.ShouldNotBeNull();
            roleClaim.Value.ShouldBe("Administrador");
        }

        [Fact]
        public void GenerateToken_ComTipoCliente_DeveConterRoleCliente()
        {
            // Arrange
            var request = new TokenRequest
            {
                Id = 1,
                Email = "cliente@example.com",
                Tipo = ETipoUsuario.Cliente
            };

            // Act
            var token = service.GenerateToken(request);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Assert
            jwtToken.ShouldNotBeNull();
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
            roleClaim.ShouldNotBeNull();
            roleClaim.Value.ShouldBe("Cliente");
        }

        [Fact]
        public void GenerateToken_DeveConterIssuer()
        {
            // Arrange
            var request = new TokenRequest
            {
                Id = 1,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente
            };

            // Act
            var token = service.GenerateToken(request);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Assert
            jwtToken.ShouldNotBeNull();
            jwtToken.Issuer.ShouldBe("PagueVeloz");
        }

        [Fact]
        public void GenerateToken_DeveConterAudience()
        {
            // Arrange
            var request = new TokenRequest
            {
                Id = 1,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente
            };

            // Act
            var token = service.GenerateToken(request);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Assert
            jwtToken.ShouldNotBeNull();
            jwtToken.Audiences.ShouldContain("Webapi");
        }

        [Fact]
        public void GenerateToken_DeveConterDataExpiracaoAmanh()
        {
            // Arrange
            var agora = DateTime.UtcNow;
            var request = new TokenRequest
            {
                Id = 1,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente
            };

            // Act
            var token = service.GenerateToken(request);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Assert
            jwtToken.ShouldNotBeNull();
            jwtToken.ValidTo.ShouldBeGreaterThan(agora.AddHours(23));
            jwtToken.ValidTo.ShouldBeLessThan(agora.AddDays(1).AddHours(1));
        }

        [Fact]
        public void GenerateToken_TokensDiferentesDevemSerDiferentes()
        {
            // Arrange
            var request1 = new TokenRequest
            {
                Id = 1,
                Email = "usuario1@example.com",
                Tipo = ETipoUsuario.Cliente
            };

            var request2 = new TokenRequest
            {
                Id = 2,
                Email = "usuario2@example.com",
                Tipo = ETipoUsuario.Gerente
            };

            // Act
            var token1 = service.GenerateToken(request1);
            var token2 = service.GenerateToken(request2);

            // Assert
            token1.ShouldNotBe(token2);
        }

        [Fact]
        public void GenerateToken_DeveSerValidoJWT()
        {
            // Arrange
            var request = new TokenRequest
            {
                Id = 1,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente
            };

            // Act
            var token = service.GenerateToken(request);
            var handler = new JwtSecurityTokenHandler();

            // Assert
            var canRead = handler.CanReadToken(token);
            canRead.ShouldBeTrue();
        }

        [Fact]
        public void GenerateToken_ComEmailVazio_DeveRetornarToken()
        {
            // Arrange
            var request = new TokenRequest
            {
                Id = 1,
                Email = "",
                Tipo = ETipoUsuario.Cliente
            };

            // Act
            var token = service.GenerateToken(request);

            // Assert
            token.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public void GenerateToken_ComIdZero_DeveRetornarToken()
        {
            // Arrange
            var request = new TokenRequest
            {
                Id = 0,
                Email = "usuario@example.com",
                Tipo = ETipoUsuario.Cliente
            };

            // Act
            var token = service.GenerateToken(request);

            // Assert
            token.ShouldNotBeNullOrEmpty();
        }
    }
}
