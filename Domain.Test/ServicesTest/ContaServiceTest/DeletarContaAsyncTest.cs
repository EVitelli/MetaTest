using Domain.Enums;
using Domain.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;

namespace Domain.Test.ServicesTest.ContaServiceTest
{
    public class DeletarContaAsyncTest : BaseContaServiceTest
    {
        [Fact]
        public async Task DeletarContaAsync_ComContaSemSaldoOuReserva_DeveDeleteComSucesso()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 0,
                Reservado = 0,
                SaldoCredito = 1000m,
                LimiteCredito = 1000m,
                Status = EStatus.Ativo,
                DeletadoEm = DateTime.UtcNow
            };

            var request = new DeletarContaRequest
            {
                IdConta = 1,
                Operador = "usuario1"
            };

            repository.BuscarContaPorIdAsync(1).Returns(contaEntity);
            repository.DeletarContaAsync(Arg.Any<Domain.Entities.Conta>()).Returns(contaEntity);

            // Act
            var response = await service.DeletarContaAsync(request);

            // Assert
            response.ShouldNotBeNull();
            response.Id.ShouldBe((uint)1);
            response.Codigo.ShouldBe("1234");
            await repository.Received(1).DeletarContaAsync(Arg.Any<Domain.Entities.Conta>());
        }

        [Fact]
        public async Task DeletarContaAsync_ComContaNaoEncontrada_DeveRetornarNulo()
        {
            // Arrange
            var request = new DeletarContaRequest
            {
                IdConta = 999,
                Operador = "usuario1"
            };

            repository.BuscarContaPorIdAsync(999).Returns((Domain.Entities.Conta?)null);

            // Act
            var response = await service.DeletarContaAsync(request);

            // Assert
            response.ShouldBeNull();
            await repository.DidNotReceive().DeletarContaAsync(Arg.Any<Domain.Entities.Conta>());
        }

        [Fact]
        public async Task DeletarContaAsync_ComContaComSaldo_DeveLancarInvalidOperationException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 500m,
                Reservado = 0,
                SaldoCredito = 1000m,
                LimiteCredito = 1000m,
                Status = EStatus.Ativo
            };

            var request = new DeletarContaRequest
            {
                IdConta = 1,
                Operador = "usuario1"
            };

            repository.BuscarContaPorIdAsync(1).Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<InvalidOperationException>(() => service.DeletarContaAsync(request));
            exception.Message.ShouldContain("Usuário possui contas com saldo ou valores reservados.");
        }

        [Fact]
        public async Task DeletarContaAsync_ComContaComReservado_DeveLancarInvalidOperationException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 0,
                Reservado = 200m,
                SaldoCredito = 1000m,
                LimiteCredito = 1000m,
                Status = EStatus.Ativo
            };

            var request = new DeletarContaRequest
            {
                IdConta = 1,
                Operador = "usuario1"
            };

            repository.BuscarContaPorIdAsync(1).Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<InvalidOperationException>(() => service.DeletarContaAsync(request));
            exception.Message.ShouldContain("Usuário possui contas com saldo ou valores reservados.");
        }

        [Fact]
        public async Task DeletarContaAsync_ComContaComSaldoCreditoDiferenteLimite_DeveLancarInvalidOperationException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 0,
                Reservado = 0,
                SaldoCredito = 500m,
                LimiteCredito = 1000m,
                Status = EStatus.Ativo
            };

            var request = new DeletarContaRequest
            {
                IdConta = 1,
                Operador = "usuario1"
            };

            repository.BuscarContaPorIdAsync(1).Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<InvalidOperationException>(() => service.DeletarContaAsync(request));
            exception.Message.ShouldContain("Usuário possui contas com saldo ou valores reservados.");
        }

        [Fact]
        public async Task DeletarContaAsync_DevePreencherOperador()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 0,
                Reservado = 0,
                SaldoCredito = 1000m,
                LimiteCredito = 1000m,
                Status = EStatus.Ativo,
                DeletadoEm = DateTime.UtcNow
            };

            var request = new DeletarContaRequest
            {
                IdConta = 1,
                Operador = "usuario_teste"
            };

            Domain.Entities.Conta? contaDeletada = null;
            repository.BuscarContaPorIdAsync(1).Returns(contaEntity);
            repository.DeletarContaAsync(Arg.Do<Domain.Entities.Conta>(c => contaDeletada = c)).Returns(contaEntity);

            // Act
            await service.DeletarContaAsync(request);

            // Assert
            contaDeletada.ShouldNotBeNull();
            contaDeletada.AtualizadoPor.ShouldBe("usuario_teste");
        }

        [Fact]
        public async Task DeletarContaAsync_ComSaldoZeroReservadoZeroSaldoCreditoZero_DeveDeleteComSucesso()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 0,
                Reservado = 0,
                SaldoCredito = 0m,
                LimiteCredito = 0m,
                Status = EStatus.Ativo,
                DeletadoEm = DateTime.UtcNow
            };

            var request = new DeletarContaRequest
            {
                IdConta = 1,
                Operador = "usuario1"
            };

            repository.BuscarContaPorIdAsync(1).Returns(contaEntity);
            repository.DeletarContaAsync(Arg.Any<Domain.Entities.Conta>()).Returns(contaEntity);

            // Act
            var response = await service.DeletarContaAsync(request);

            // Assert
            response.ShouldNotBeNull();
            response.Id.ShouldBe((uint)1);
        }
    }
}
