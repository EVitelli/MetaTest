using Domain.Enums;
using Domain.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;

namespace Domain.Test.ServicesTest.ContaServiceTest
{
    public class AtualizarLimiteCreditoAsyncTest : BaseContaServiceTest
    {
        [Fact]
        public async Task AtualizarLimiteCreditoAsync_ComLimiteValido_DeveAtualizarComSucesso()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                LimiteCredito = 1000m,
                SaldoCredito = 500m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizarLimiteCreditoRequest
            {
                CodigoConta = "1234",
                LimiteCredito = 2000m,
                Operador = "usuario1"
            };

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarLimiteCreditoAsync(request);

            // Assert
            response.ShouldNotBeNull();
            response.Id.ShouldBe(contaEntity.Id);
            response.Codigo.ShouldBe(contaEntity.Codigo);
            response.LimiteCredito.ShouldBe(2000m);
            await repository.Received(1).AtualizarContaAsync(Arg.Any<Domain.Entities.Conta>());
        }

        [Fact]
        public async Task AtualizarLimiteCreditoAsync_ComLimiteAumentar_DeveManterSaltoCreditoAnterior()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                LimiteCredito = 1000m,
                SaldoCredito = 800m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizarLimiteCreditoRequest
            {
                CodigoConta = "1234",
                LimiteCredito = 2000m,
                Operador = "usuario1"
            };

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarLimiteCreditoAsync(request);

            // Assert
            contaEntity.SaldoCredito.ShouldBe(800m);
        }

        [Fact]
        public async Task AtualizarLimiteCreditoAsync_ComLimiteDiminuir_DeveReduzirSaldoCreditoSeNecessario()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                LimiteCredito = 1000m,
                SaldoCredito = 800m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizarLimiteCreditoRequest
            {
                CodigoConta = "1234",
                LimiteCredito = 500m,
                Operador = "usuario1"
            };

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarLimiteCreditoAsync(request);

            // Assert
            contaEntity.SaldoCredito.ShouldBe(500m);
        }

        [Fact]
        public async Task AtualizarLimiteCreditoAsync_ComContaNaoEncontrada_DeveLancarArgumentException()
        {
            // Arrange
            var request = new AtualizarLimiteCreditoRequest
            {
                CodigoConta = "9999",
                LimiteCredito = 2000m,
                Operador = "usuario1"
            };

            repository.BuscarContaPorCodigoAsync("9999").Returns((Domain.Entities.Conta?)null);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarLimiteCreditoAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar sreserva da conta.");
        }

        [Fact]
        public async Task AtualizarLimiteCreditoAsync_ComContaInativa_DeveLancarArgumentException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                LimiteCredito = 1000m,
                SaldoCredito = 500m,
                Status = EStatus.Inativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizarLimiteCreditoRequest
            {
                CodigoConta = "1234",
                LimiteCredito = 2000m,
                Operador = "usuario1"
            };

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarLimiteCreditoAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar sreserva da conta.");
        }

        [Fact]
        public async Task AtualizarLimiteCreditoAsync_DeveAtualizarOperador()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                LimiteCredito = 1000m,
                SaldoCredito = 500m,
                Status = EStatus.Ativo,
                AtualizadoPor = "usuario_antigo",
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizarLimiteCreditoRequest
            {
                CodigoConta = "1234",
                LimiteCredito = 2000m,
                Operador = "usuario_novo"
            };

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            await service.AtualizarLimiteCreditoAsync(request);

            // Assert
            contaEntity.AtualizadoPor.ShouldBe("usuario_novo");
        }

        [Fact]
        public async Task AtualizarLimiteCreditoAsync_ComRepositoryError_DeveLancarException()
        {
            // Arrange
            var request = new AtualizarLimiteCreditoRequest
            {
                CodigoConta = "1234",
                LimiteCredito = 2000m,
                Operador = "usuario1"
            };

            repository.BuscarContaPorCodigoAsync("1234").Throws(new Exception("Erro na base de dados"));

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(() => service.AtualizarLimiteCreditoAsync(request));
            exception.Message.ShouldContain("Erro inesperado ao atualizar reserva da conta.");
        }

        [Fact]
        public async Task AtualizarLimiteCreditoAsync_ComLimiteZero_DeveAtualizarComSucesso()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                LimiteCredito = 1000m,
                SaldoCredito = 500m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizarLimiteCreditoRequest
            {
                CodigoConta = "1234",
                LimiteCredito = 0m,
                Operador = "usuario1"
            };

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarLimiteCreditoAsync(request);

            // Assert
            response.LimiteCredito.ShouldBe(0m);
            contaEntity.SaldoCredito.ShouldBe(0m);
        }
    }
}
