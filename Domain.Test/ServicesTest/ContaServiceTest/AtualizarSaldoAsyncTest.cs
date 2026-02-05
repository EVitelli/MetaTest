using Domain.Enums;
using Domain.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;

namespace Domain.Test.ServicesTest.ContaServiceTest
{
    public class AtualizarSaldoAsyncTest : BaseContaServiceTest
    {
        [Fact]
        public async Task AtualizarSaldoAsync_ComDepositoESaldoValido_DeveAtualizarComSucesso()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 500m, EOperacaoFinanceira.Deposito);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarSaldoAsync(request);

            // Assert
            response.ShouldNotBeNull();
            response.Id.ShouldBe(contaEntity.Id);
            response.Codigo.ShouldBe(contaEntity.Codigo);
            response.Saldo.ShouldBe(1500m);
            contaEntity.Saldo.ShouldBe(1500m);
            await repository.Received(1).AtualizarContaAsync(Arg.Any<Domain.Entities.Conta>());
        }

        [Fact]
        public async Task AtualizarSaldoAsync_ComDebitoESaldoSuficiente_DeveAtualizarComSucesso()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 300m, EOperacaoFinanceira.Debito);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarSaldoAsync(request);

            // Assert
            response.ShouldNotBeNull();
            response.Codigo.ShouldBe("1234");
            response.Saldo.ShouldBe(700m);
            contaEntity.Saldo.ShouldBe(700m);
            await repository.Received(1).AtualizarContaAsync(Arg.Any<Domain.Entities.Conta>());
        }

        [Fact]
        public async Task AtualizarSaldoAsync_ComDebitoESaldoInsuficiente_DeveLancarArgumentException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 100m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 500m, EOperacaoFinanceira.Debito);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarSaldoAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar saldo da conta.");
        }

        [Fact]
        public async Task AtualizarSaldoAsync_ComOperacaoInvalida_DeveLancarArgumentException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 500m, EOperacaoFinanceira.Aplicacao);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarSaldoAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar saldo da conta.");
        }

        [Fact]
        public async Task AtualizarSaldoAsync_ComContaNaoEncontrada_DeveLancarArgumentException()
        {
            // Arrange
            var request = new AtualizaValorContaRequest("9999", 500m, EOperacaoFinanceira.Deposito);

            repository.BuscarContaPorCodigoAsync("9999").Returns((Domain.Entities.Conta?)null);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarSaldoAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar saldo da conta.");
        }

        [Fact]
        public async Task AtualizarSaldoAsync_ComContaInativa_DeveLancarArgumentException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Status = EStatus.Inativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 500m, EOperacaoFinanceira.Deposito);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarSaldoAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar saldo da conta.");
        }

        [Fact]
        public async Task AtualizarSaldoAsync_ComRepositoryError_DeveLancarException()
        {
            // Arrange
            var request = new AtualizaValorContaRequest("1234", 500m, EOperacaoFinanceira.Deposito);

            repository.BuscarContaPorCodigoAsync("1234").Throws(new Exception("Erro na base de dados"));

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(() => service.AtualizarSaldoAsync(request));
            exception.Message.ShouldContain("Erro inesperado ao atualizar saldo da conta.");
        }

        [Fact]
        public async Task AtualizarSaldoAsync_ComDepositoZero_DeveAtualizarSaldoNormalmente()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 0m, EOperacaoFinanceira.Deposito);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarSaldoAsync(request);

            // Assert
            response.Saldo.ShouldBe(1000m);
        }

        [Fact]
        public async Task AtualizarSaldoAsync_ComDebitoQueZeraSaldo_DeveAtualizarComSucesso()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 500m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 500m, EOperacaoFinanceira.Debito);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarSaldoAsync(request);

            // Assert
            response.Saldo.ShouldBe(0m);
        }
    }
}
