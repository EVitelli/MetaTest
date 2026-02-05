using Domain.Enums;
using Domain.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;

namespace Domain.Test.ServicesTest.ContaServiceTest
{
    public class CriarContaAsyncTest : BaseContaServiceTest
    {
        [Fact]
        public async Task CriarContaAsync_ComDadosValidos_DeveCreateComSucesso()
        {
            // Arrange
            var novaConta = new Domain.Entities.Conta
            {
                Id = 1,
                IdUsuarioCliente = 100,
                IdUsuarioGerente = 200,
                Codigo = "1234",
                LimiteCredito = 5000m,
                Saldo = 0,
                Reservado = 0,
                SaldoCredito = 5000m,
                Status = EStatus.Ativo,
                CriadoEm = DateTime.UtcNow,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new CriarContaRequest
            {
                ClienteId = 100,
                GerenteId = 200,
                LimiteCredito = 5000m,
                Operador = "usuario1"
            };

            repository.CriarContaAsync(Arg.Any<Domain.Entities.Conta>()).Returns(novaConta);

            // Act
            var response = await service.CriarContaAsync(request);

            // Assert
            response.ShouldNotBeNull();
            response.Id.ShouldBe((uint)1);
            response.ClienteId.ShouldBe((uint)100);
            response.GerenteId.ShouldBe((uint)200);
            response.LimiteCredito.ShouldBe(5000m);
            response.Codigo.ShouldBe("1234");
            await repository.Received(1).CriarContaAsync(Arg.Any<Domain.Entities.Conta>());
        }

        [Fact]
        public async Task CriarContaAsync_ComClienteIgualGerente_DeveLancarArgumentException()
        {
            // Arrange
            var request = new CriarContaRequest
            {
                ClienteId = 100,
                GerenteId = 100,
                LimiteCredito = 5000m,
                Operador = "usuario1"
            };

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.CriarContaAsync(request));
            exception.Message.ShouldContain("Erro ao criar uma nova conta.");
        }

        [Fact]
        public async Task CriarContaAsync_ComRequestNulo_DeveLancarArgumentNullException()
        {
            // Act & Assert
            await Should.ThrowAsync<ArgumentNullException>(() => service.CriarContaAsync(null));
        }

        [Fact]
        public async Task CriarContaAsync_DeveDefinirSaldoZero()
        {
            // Arrange
            var novaConta = new Domain.Entities.Conta
            {
                Id = 1,
                IdUsuarioCliente = 100,
                IdUsuarioGerente = 200,
                Codigo = "1234",
                LimiteCredito = 5000m,
                Saldo = 0,
                Reservado = 0,
                SaldoCredito = 5000m,
                Status = EStatus.Ativo,
                CriadoEm = DateTime.UtcNow,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new CriarContaRequest
            {
                ClienteId = 100,
                GerenteId = 200,
                LimiteCredito = 5000m,
                Operador = "usuario1"
            };

            Domain.Entities.Conta? contaCriada = null;
            repository.CriarContaAsync(Arg.Do<Domain.Entities.Conta>(c => contaCriada = c)).Returns(novaConta);

            // Act
            await service.CriarContaAsync(request);

            // Assert
            contaCriada.ShouldNotBeNull();
            contaCriada.Saldo.ShouldBe(0);
            contaCriada.Reservado.ShouldBe(0);
        }

        [Fact]
        public async Task CriarContaAsync_DeveDefinirStatusAtivo()
        {
            // Arrange
            var novaConta = new Domain.Entities.Conta
            {
                Id = 1,
                IdUsuarioCliente = 100,
                IdUsuarioGerente = 200,
                Codigo = "1234",
                LimiteCredito = 5000m,
                Saldo = 0,
                Reservado = 0,
                SaldoCredito = 5000m,
                Status = EStatus.Ativo,
                CriadoEm = DateTime.UtcNow,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new CriarContaRequest
            {
                ClienteId = 100,
                GerenteId = 200,
                LimiteCredito = 5000m,
                Operador = "usuario1"
            };

            Domain.Entities.Conta? contaCriada = null;
            repository.CriarContaAsync(Arg.Do<Domain.Entities.Conta>(c => contaCriada = c)).Returns(novaConta);

            // Act
            await service.CriarContaAsync(request);

            // Assert
            contaCriada.ShouldNotBeNull();
            contaCriada.Status.ShouldBe(EStatus.Ativo);
        }

        [Fact]
        public async Task CriarContaAsync_DeveDefinirSaldoCreditoIgualLimiteCredito()
        {
            // Arrange
            var novaConta = new Domain.Entities.Conta
            {
                Id = 1,
                IdUsuarioCliente = 100,
                IdUsuarioGerente = 200,
                Codigo = "1234",
                LimiteCredito = 5000m,
                Saldo = 0,
                Reservado = 0,
                SaldoCredito = 5000m,
                Status = EStatus.Ativo,
                CriadoEm = DateTime.UtcNow,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new CriarContaRequest
            {
                ClienteId = 100,
                GerenteId = 200,
                LimiteCredito = 5000m,
                Operador = "usuario1"
            };

            Domain.Entities.Conta? contaCriada = null;
            repository.CriarContaAsync(Arg.Do<Domain.Entities.Conta>(c => contaCriada = c)).Returns(novaConta);

            // Act
            await service.CriarContaAsync(request);

            // Assert
            contaCriada.ShouldNotBeNull();
            contaCriada.SaldoCredito.ShouldBe(contaCriada.LimiteCredito);
        }

        [Fact]
        public async Task CriarContaAsync_DevePreencherOperador()
        {
            // Arrange
            var novaConta = new Domain.Entities.Conta
            {
                Id = 1,
                IdUsuarioCliente = 100,
                IdUsuarioGerente = 200,
                Codigo = "1234",
                LimiteCredito = 5000m,
                Saldo = 0,
                Reservado = 0,
                SaldoCredito = 5000m,
                Status = EStatus.Ativo,
                CriadoEm = DateTime.UtcNow,
                AtualizadoEm = DateTime.UtcNow,
                AtualizadoPor = "usuario1"
            };

            var request = new CriarContaRequest
            {
                ClienteId = 100,
                GerenteId = 200,
                LimiteCredito = 5000m,
                Operador = "usuario1"
            };

            Domain.Entities.Conta? contaCriada = null;
            repository.CriarContaAsync(Arg.Do<Domain.Entities.Conta>(c => contaCriada = c)).Returns(novaConta);

            // Act
            await service.CriarContaAsync(request);

            // Assert
            contaCriada.ShouldNotBeNull();
            contaCriada.AtualizadoPor.ShouldBe("usuario1");
        }

        [Fact]
        public async Task CriarContaAsync_ComRepositoryError_DeveLancarException()
        {
            // Arrange
            var request = new CriarContaRequest
            {
                ClienteId = 100,
                GerenteId = 200,
                LimiteCredito = 5000m,
                Operador = "usuario1"
            };

            repository.CriarContaAsync(Arg.Any<Domain.Entities.Conta>())
                .Throws(new Exception("Erro na base de dados"));

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(() => service.CriarContaAsync(request));
            exception.Message.ShouldContain("Erro inesperado ao criar uma nova conta.");
        }

        [Fact]
        public async Task CriarContaAsync_ComLimiteCreditoZero_DeveAtualizarComSucesso()
        {
            // Arrange
            var novaConta = new Domain.Entities.Conta
            {
                Id = 1,
                IdUsuarioCliente = 100,
                IdUsuarioGerente = 200,
                Codigo = "1234",
                LimiteCredito = 0m,
                Saldo = 0,
                Reservado = 0,
                SaldoCredito = 0m,
                Status = EStatus.Ativo,
                CriadoEm = DateTime.UtcNow,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new CriarContaRequest
            {
                ClienteId = 100,
                GerenteId = 200,
                LimiteCredito = 0m,
                Operador = "usuario1"
            };

            repository.CriarContaAsync(Arg.Any<Domain.Entities.Conta>()).Returns(novaConta);

            // Act
            var response = await service.CriarContaAsync(request);

            // Assert
            response.LimiteCredito.ShouldBe(0m);
        }
    }
}
