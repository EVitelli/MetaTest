using Domain.Enums;
using Domain.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;

namespace Domain.Test.ServicesTest.ContaServiceTest
{
    public class AtualizarReservaAsyncTest : BaseContaServiceTest
    {
        [Fact]
        public async Task AtualizarReservaAsync_ComAplicacaoESaldoSuficiente_DeveAtualizarComSucesso()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Reservado = 500m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Aplicacao);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarReservaAsync(request);

            // Assert
            response.ShouldNotBeNull();
            response.Id.ShouldBe(contaEntity.Id);
            response.Codigo.ShouldBe(contaEntity.Codigo);
            response.Saldo.ShouldBe(800m);
            response.Reservado.ShouldBe(700m);
            contaEntity.Saldo.ShouldBe(800m);
            contaEntity.Reservado.ShouldBe(700m);
            await repository.Received(1).AtualizarContaAsync(Arg.Any<Domain.Entities.Conta>());
        }

        [Fact]
        public async Task AtualizarReservaAsync_ComAplicacaoESaldoInsuficiente_DeveLancarArgumentException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 100m,
                Reservado = 500m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Aplicacao);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarReservaAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar sreserva da conta.");
        }

        [Fact]
        public async Task AtualizarReservaAsync_ComResgateEValorReservadoSuficiente_DeveAtualizarComSucesso()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 500m,
                Reservado = 1000m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Resgate);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarReservaAsync(request);

            // Assert
            response.ShouldNotBeNull();
            response.Id.ShouldBe(contaEntity.Id);
            response.Codigo.ShouldBe(contaEntity.Codigo);
            response.Saldo.ShouldBe(700m);
            response.Reservado.ShouldBe(800m);
            contaEntity.Saldo.ShouldBe(700m);
            contaEntity.Reservado.ShouldBe(800m);
            await repository.Received(1).AtualizarContaAsync(Arg.Any<Domain.Entities.Conta>());
        }

        [Fact]
        public async Task AtualizarReservaAsync_ComResgateEValorReservadoInsuficiente_DeveLancarArgumentException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 500m,
                Reservado = 100m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Resgate);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarReservaAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar sreserva da conta.");
        }

        [Fact]
        public async Task AtualizarReservaAsync_ComOperacaoInvalida_DeveLancarArgumentException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Reservado = 500m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Deposito);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarReservaAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar sreserva da conta.");
        }

        [Fact]
        public async Task AtualizarReservaAsync_ComContaNaoEncontrada_DeveLancarArgumentException()
        {
            // Arrange
            var request = new AtualizaValorContaRequest("9999", 200m, EOperacaoFinanceira.Aplicacao);

            repository.BuscarContaPorCodigoAsync("9999").Returns((Domain.Entities.Conta?)null);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarReservaAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar sreserva da conta.");
        }

        [Fact]
        public async Task AtualizarReservaAsync_ComContaInativa_DeveLancarArgumentException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Reservado = 500m,
                Status = EStatus.Inativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Aplicacao);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarReservaAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar sreserva da conta.");
        }

        [Fact]
        public async Task AtualizarReservaAsync_ComRepositoryError_DeveLancarException()
        {
            // Arrange
            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Aplicacao);

            repository.BuscarContaPorCodigoAsync("1234").Throws(new Exception("Erro na base de dados"));

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(() => service.AtualizarReservaAsync(request));
            exception.Message.ShouldContain("Erro inesperado ao atualizar reserva da conta.");
        }

        [Fact]
        public async Task AtualizarReservaAsync_ComAplicacaoESaldoExato_DeveZerarSaldo()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 200m,
                Reservado = 500m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Aplicacao);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarReservaAsync(request);

            // Assert
            response.Saldo.ShouldBe(0m);
            response.Reservado.ShouldBe(700m);
        }

        [Fact]
        public async Task AtualizarReservaAsync_ComResgateEValorReservadoExato_DeveZerarReservado()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 500m,
                Reservado = 200m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Resgate);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act
            var response = await service.AtualizarReservaAsync(request);

            // Assert
            response.Saldo.ShouldBe(700m);
            response.Reservado.ShouldBe(0m);
        }

        [Fact]
        public async Task AtualizarReservaAsync_ComCreditoComOperacao_DeveLancarArgumentException()
        {
            // Arrange
            var contaEntity = new Domain.Entities.Conta
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1000m,
                Reservado = 500m,
                Status = EStatus.Ativo,
                AtualizadoEm = DateTime.UtcNow
            };

            var request = new AtualizaValorContaRequest("1234", 200m, EOperacaoFinanceira.Credito);

            repository.BuscarContaPorCodigoAsync("1234").Returns(contaEntity);

            // Act & Assert
            var exception = await Should.ThrowAsync<ArgumentException>(() => service.AtualizarReservaAsync(request));
            exception.Message.ShouldContain("Erro ao atualizar sreserva da conta.");
        }
    }
}
