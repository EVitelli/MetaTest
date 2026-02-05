using Domain.Enums;
using Domain.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;

namespace Domain.Test.ServicesTest.ContaServiceTest
{
    public class AtualizarSaldoCreditoAsyncTest : BaseContaServiceTest
    {
        [Fact]
        public async Task AtualizarSaldoCreditoAsync_ComPagamentoESaldoValido_DeveAtualizarComSucesso()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                SaldoCredito = 300m,
                LimiteCredito = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Pagamento);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarSaldoCreditoAsync(request);

            // Assert
            response.ShouldNotBeNull();
            response.Id.ShouldBe(contaEntity.Id);
            response.Codigo.ShouldBe("1234");
            response.SaldoCredito.ShouldBe(500m);
            contaEntity.SaldoCredito.ShouldBe(500m);
            await repository.Received(1).AtualizarContaAsync(Arg.Any<Domain.Entities.Conta>());
        }

        [Fact]
        public async Task AtualizarSaldoCreditoAsync_ComCreditoESaldoSuficiente_DeveAtualizarComSucesso()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                SaldoCredito = 800m,
                LimiteCredito = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 300m, EOperacaoFinanceira.Credito);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarSaldoCreditoAsync(request);

            // Assert
            response.ShouldNotBeNull();
            response.SaldoCredito.ShouldBe(500m);
            contaEntity.SaldoCredito.ShouldBe(500m);
            await repository.Received(1).AtualizarContaAsync(Arg.Any<Domain.Entities.Conta>());
        }

        [Fact]
        public async Task AtualizarSaldoCreditoAsync_ComCreditoESaldoInsuficiente_DeveLancarArgumentException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                SaldoCredito = 100m,
                LimiteCredito = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 300m, EOperacaoFinanceira.Credito);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarSaldoCreditoAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar saldo crédito da conta.");
        }

        [Fact]
        public async Task AtualizarSaldoCreditoAsync_ComPagamentoSuperiorAoLimite_DeveLancarArgumentException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                SaldoCredito = 500m,
                LimiteCredito = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 600m, EOperacaoFinanceira.Pagamento);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarSaldoCreditoAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar saldo crédito da conta.");
        }

        [Fact]
        public async Task AtualizarSaldoCreditoAsync_ComOperacaoInvalida_DeveLancarArgumentException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                SaldoCredito = 500m,
                LimiteCredito = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Deposito);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarSaldoCreditoAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar saldo crédito da conta.");
        }

        [Fact]
        public async Task AtualizarSaldoCreditoAsync_ComContaNaoEncontrada_DeveLancarArgumentException()
        {
            // Arrange
            var request = new AtualizaValorContaRequest("9999", 200m, EOperacaoFinanceira.Pagamento);

            repository.BuscarContaPorCodigoAsync("9999").Returns((Domain.Entities.Conta?)null);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarSaldoCreditoAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar saldo crédito da conta.");
        }

        [Fact]
        public async Task AtualizarSaldoCreditoAsync_ComContaInativa_DeveLancarArgumentException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                SaldoCredito = 500m,
                LimiteCredito = 1000m,
                Status = EStatus.Inativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Pagamento);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarSaldoCreditoAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar saldo crédito da conta.");
        }

        [Fact]
        public async Task AtualizarSaldoCreditoAsync_ComRepositoryError_DeveLancarException()
        {
            // Arrange
            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Pagamento);

            repository.BuscarContaPorCodigoAsync("1234").Throws(new Exception("Erro na base de dados"));

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(() => service.AtualizarSaldoCreditoAsync(request));
            exception.Message.ShouldContain("Erro inesperado ao atualizar saldo crédito da conta.");
        }

        [Fact]
        public async Task AtualizarSaldoCreditoAsync_ComPagamentoQueZeraSaldoCredito_DeveAtualizarComSucesso()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                SaldoCredito = 200m,
                LimiteCredito = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Pagamento);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarSaldoCreditoAsync(request);

            // Assert
            response.SaldoCredito.ShouldBe(400m);
        }

        [Fact]
        public async Task AtualizarSaldoCreditoAsync_ComPagamentoAteLimite_DeveAtualizarComSucesso()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                SaldoCredito = 500m,
                LimiteCredito = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 500m, EOperacaoFinanceira.Pagamento);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarSaldoCreditoAsync(request);

            // Assert
            response.SaldoCredito.ShouldBe(1000m);
        }
    }
}
