using Domain.Enums;
using Domain.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;

namespace Domain.Test.ServicesTest.ContaServiceTest
{
    public class ProcessarTransferenciaAsyncTest : BaseContaServiceTest
    {
        [Fact]
        public async Task ProcessarTransferenciaAsync_ComDadosValidos_DeveTransferirComSucesso()
        {
            // Arrange
            var contaOrigem = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var contaDestino = new Domain.Entities.Conta
            {
                Id = 2,
                Codigo = "5678",
                Saldo = 500m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new TransferenciaRequest
            {
                CodigoContaOrigem = "1234",
                CodigoContaDestino = "5678",
                Valor = 300m
            };

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaOrigem);
            repository.BuscarContaPorCodigoAsync("5678").Returns(contaDestino);

            // Act
            var response = await service.ProcessarTransferenciaAsync(request);

            // Assert
            response.ShouldNotBeNull();
            response.CodigoContaOrigem.ShouldBe("1234");
            response.CodigoContaDestino.ShouldBe("5678");
            response.SaldoContaOrigem.ShouldBe(700m);
            response.SaldoContaDestino.ShouldBe(800m);
            contaOrigem.Saldo.ShouldBe(700m);
            contaDestino.Saldo.ShouldBe(800m);
            await repository.Received(2).AtualizarContaAsync(Arg.Any<Domain.Entities.Conta>());
        }

        [Fact]
        public async Task ProcessarTransferenciaAsync_ComContaOrigemNaoEncontrada_DeveLancarArgumentException()
        {
            // Arrange
            var request = new TransferenciaRequest
            {
                CodigoContaOrigem = "9999",
                CodigoContaDestino = "5678",
                Valor = 300m
            };

            repository.BuscarContaPorCodigoAsync("9999").Returns((Domain.Entities.Conta?)null);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.ProcessarTransferenciaAsync(request));
            exception.Message.ShouldContain("Erro ao processar transferência.");
        }

        [Fact]
        public async Task ProcessarTransferenciaAsync_ComContaDestinoNaoEncontrada_DeveLancarArgumentException()
        {
            // Arrange
            var contaOrigem = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new TransferenciaRequest
            {
                CodigoContaOrigem = "1234",
                CodigoContaDestino = "9999",
                Valor = 300m
            };

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaOrigem);
            repository.BuscarContaPorCodigoAsync("9999").Returns((Domain.Entities.Conta?)null);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.ProcessarTransferenciaAsync(request));
            exception.Message.ShouldContain("Erro ao processar transferência.");
        }

        [Fact]
        public async Task ProcessarTransferenciaAsync_ComContaOrigemInativa_DeveLancarArgumentException()
        {
            // Arrange
            var contaOrigem = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Status = EStatus.Inativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var contaDestino = new Domain.Entities.Conta
            {
                Id = 2,
                Codigo = "5678",
                Saldo = 500m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new TransferenciaRequest
            {
                CodigoContaOrigem = "1234",
                CodigoContaDestino = "5678",
                Valor = 300m
            };

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaOrigem);
            repository.BuscarContaPorCodigoAsync("5678").Returns(contaDestino);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.ProcessarTransferenciaAsync(request));
            exception.Message.ShouldContain("Erro ao processar transferência.");
        }

        [Fact]
        public async Task ProcessarTransferenciaAsync_ComContaDestinoInativa_DeveLancarArgumentException()
        {
            // Arrange
            var contaOrigem = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var contaDestino = new Domain.Entities.Conta
            {
                Id = 2,
                Codigo = "5678",
                Saldo = 500m,
                Status = EStatus.Inativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new TransferenciaRequest
            {
                CodigoContaOrigem = "1234",
                CodigoContaDestino = "5678",
                Valor = 300m
            };

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaOrigem);
            repository.BuscarContaPorCodigoAsync("5678").Returns(contaDestino);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.ProcessarTransferenciaAsync(request));
            exception.Message.ShouldContain("Erro ao processar transferência.");
        }

        [Fact]
        public async Task ProcessarTransferenciaAsync_ComSaldoOrigemInsuficiente_DeveLancarArgumentException()
        {
            // Arrange
            var contaOrigem = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 100m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var contaDestino = new Domain.Entities.Conta
            {
                Id = 2,
                Codigo = "5678",
                Saldo = 500m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new TransferenciaRequest
            {
                CodigoContaOrigem = "1234",
                CodigoContaDestino = "5678",
                Valor = 300m
            };

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaOrigem);
            repository.BuscarContaPorCodigoAsync("5678").Returns(contaDestino);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.ProcessarTransferenciaAsync(request));
            exception.Message.ShouldContain("Erro ao processar transferência.");
        }

        [Fact]
        public async Task ProcessarTransferenciaAsync_ComTransferenciaCompleta_DeveZerarSaldoOrigem()
        {
            // Arrange
            var contaOrigem = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 300m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var contaDestino = new Domain.Entities.Conta
            {
                Id = 2,
                Codigo = "5678",
                Saldo = 500m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new TransferenciaRequest
            {
                CodigoContaOrigem = "1234",
                CodigoContaDestino = "5678",
                Valor = 300m
            };

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaOrigem);
            repository.BuscarContaPorCodigoAsync("5678").Returns(contaDestino);

            // Act
            var response = await service.ProcessarTransferenciaAsync(request);

            // Assert
            response.SaldoContaOrigem.ShouldBe(0m);
            response.SaldoContaDestino.ShouldBe(800m);
        }

        [Fact]
        public async Task ProcessarTransferenciaAsync_ComRepositoryError_DeveLancarException()
        {
            // Arrange
            var request = new TransferenciaRequest
            {
                CodigoContaOrigem = "1234",
                CodigoContaDestino = "5678",
                Valor = 300m
            };

            repository.BuscarContaPorCodigoAsync("1234").Throws(new Exception("Erro na base de dados"));

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(() => service.ProcessarTransferenciaAsync(request));
            exception.Message.ShouldContain("Erro inesperado ao processar transferência.");
        }

        [Fact]
        public async Task ProcessarTransferenciaAsync_ComTransferenciaZero_DeveRetornarMesmoSaldo()
        {
            // Arrange
            var contaOrigem = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var contaDestino = new Domain.Entities.Conta
            {
                Id = 2,
                Codigo = "5678",
                Saldo = 500m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new TransferenciaRequest
            {
                CodigoContaOrigem = "1234",
                CodigoContaDestino = "5678",
                Valor = 0m
            };

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaOrigem);
            repository.BuscarContaPorCodigoAsync("5678").Returns(contaDestino);

            // Act
            var response = await service.ProcessarTransferenciaAsync(request);

            // Assert
            response.SaldoContaOrigem.ShouldBe(1000m);
            response.SaldoContaDestino.ShouldBe(500m);
        }
    }
}
