using Domain.Enums;
using Domain.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;

namespace Domain.Test.ServicesTest.ContaServiceTest
{
    public class BuscarContaPorClienteAsyncTest : BaseContaServiceTest
    {
        [Fact]
        public async Task BuscarContaPorClienteAsync_ComClienteComContas_DeveRetornarContas()
        {
            // Arrange
            var contasEntity = new List<Domain.Entities.Conta>
            {
                new Domain.Entities.Conta
                {
                    Id = 1,
                    Codigo = "1234",
                    IdUsuarioCliente = 100,
                    Saldo = 1000m,
                    Status = EStatus.Ativo
                },
                new Domain.Entities.Conta
                {
                    Id = 2,
                    Codigo = "5678",
                    IdUsuarioCliente = 100,
                    Saldo = 2000m,
                    Status = EStatus.Ativo
                }
            };

            repository.BuscarContaPorClienteAsync(100).Returns(contasEntity);

            // Act
            var response = await service.BuscarContaPorClienteAsync(100);

            // Assert
            response.ShouldNotBeNull();
            response.Count.ShouldBe(2);
            response[0].Codigo.ShouldBe("1234");
            response[0].Saldo.ShouldBe(1000m);
            response[1].Codigo.ShouldBe("5678");
            response[1].Saldo.ShouldBe(2000m);
            await repository.Received(1).BuscarContaPorClienteAsync(100);
        }

        [Fact]
        public async Task BuscarContaPorClienteAsync_ComClienteSemContas_DeveRetornarNulo()
        {
            // Arrange
            repository.BuscarContaPorClienteAsync(999).Returns(new List<Domain.Entities.Conta>());

            // Act
            var response = await service.BuscarContaPorClienteAsync(999);

            // Assert
            response.ShouldBeNull();
            await repository.Received(1).BuscarContaPorClienteAsync(999);
        }

        [Fact]
        public async Task BuscarContaPorClienteAsync_ComClienteQuandoRepositoryRetornaNull_DeveRetornarNulo()
        {
            // Arrange
            repository.BuscarContaPorClienteAsync(999).Returns((List<Domain.Entities.Conta>?)null);

            // Act
            var response = await service.BuscarContaPorClienteAsync(999);

            // Assert
            response.ShouldBeNull();
            await repository.Received(1).BuscarContaPorClienteAsync(999);
        }

        [Fact]
        public async Task BuscarContaPorClienteAsync_ComUmaContaApenas_DeveRetornarLista()
        {
            // Arrange
            var contasEntity = new List<Domain.Entities.Conta>
            {
                new Domain.Entities.Conta
                {
                    Id = 1,
                    Codigo = "1234",
                    IdUsuarioCliente = 50,
                    Saldo = 500m,
                    Status = EStatus.Ativo
                }
            };

            repository.BuscarContaPorClienteAsync(50).Returns(contasEntity);

            // Act
            var response = await service.BuscarContaPorClienteAsync(50);

            // Assert
            response.ShouldNotBeNull();
            response.Count.ShouldBe(1);
            response[0].Codigo.ShouldBe("1234");
        }

        [Fact]
        public async Task BuscarContaPorClienteAsync_ComRepositoryError_DeveLancarException()
        {
            // Arrange
            repository.BuscarContaPorClienteAsync(100).Throws(new Exception("Erro na base de dados"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(() => service.BuscarContaPorClienteAsync(100));
        }

        [Fact]
        public async Task BuscarContaPorClienteAsync_ComContasInativos_DeveRetornarTodasAsContas()
        {
            // Arrange
            var contasEntity = new List<Domain.Entities.Conta>
            {
                new Domain.Entities.Conta
                {
                    Id = 1,
                    Codigo = "1234",
                    IdUsuarioCliente = 100,
                    Saldo = 1000m,
                    Status = EStatus.Inativo
                },
                new Domain.Entities.Conta
                {
                    Id = 2,
                    Codigo = "5678",
                    IdUsuarioCliente = 100,
                    Saldo = 2000m,
                    Status = EStatus.Ativo
                }
            };

            repository.BuscarContaPorClienteAsync(100).Returns(contasEntity);

            // Act
            var response = await service.BuscarContaPorClienteAsync(100);

            // Assert
            response.ShouldNotBeNull();
            response.Count.ShouldBe(2);
            response[0].Status.ShouldBe(EStatus.Inativo);
            response[1].Status.ShouldBe(EStatus.Ativo);
        }
    }
}
