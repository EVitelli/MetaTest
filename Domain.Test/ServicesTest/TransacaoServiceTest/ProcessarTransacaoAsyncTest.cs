using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;

namespace Domain.Test.ServicesTest.TransacaoServiceTest
{
    public class ProcessarTransacaoAsyncTest : BaseTransacaoServiceTest
    {
        [Theory]
        [InlineData(EOperacaoFinanceira.Deposito)]
        [InlineData(EOperacaoFinanceira.Debito)]
        public async Task ProcessarTransacaoAsync_ComDepositoOuDebitoValido_DeveRetornarTrue(EOperacaoFinanceira op)
        {
            // Arrange
            var request = new TransacaoRequest
            {
                IdCliente = 100,
                CodigoConta = "1234",
                TipoOperacao = op,
            };

            var saldoResponse = new AtualizaSaldoResponse
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1500m,
                DataAtualizacao = DateTime.UtcNow
            };

            if(op == EOperacaoFinanceira.Deposito)
            {
                request.Valor = 500m;
                saldoResponse.Saldo = 1500m;
            }
            else
            {
                request.Valor = 300m;
                saldoResponse.Saldo = 700m;
            }


            await repository.CriarTransacaoAsync(Arg.Any<Transacao>());
            contaService.AtualizarSaldoAsync(Arg.Any<AtualizaValorContaRequest>()).Returns(saldoResponse);
            await repository.AtualizarTransacaoAsync(Arg.Any<Transacao>());

            // Act
            var response = await service.ProcessarTransacaoAsync(request);

            // Assert
            response.ShouldBeTrue();
            await repository.Received(1).CriarTransacaoAsync(Arg.Any<Transacao>());
            await contaService.Received(1).AtualizarSaldoAsync(Arg.Any<AtualizaValorContaRequest>());
            await repository.Received(1).AtualizarTransacaoAsync(Arg.Any<Transacao>());
        }

        [Theory]
        [InlineData(EOperacaoFinanceira.Aplicacao)]
        [InlineData(EOperacaoFinanceira.Resgate)]
        public async Task ProcessarTransacaoAsync_ComAplicacaoOuResgateValida_DeveRetornarTrue(EOperacaoFinanceira op)
        {
            // Arrange
            var request = new TransacaoRequest
            {
                IdCliente = 100,
                CodigoConta = "1234",
                TipoOperacao = op,
            };

            var reservaResponse = new AtualizaReservaResponse
            {
                Id = 1,
                Codigo = "1234",
                Reservado = 700m,
                DataAtualizacao = DateTime.UtcNow
            };

            if (op == EOperacaoFinanceira.Aplicacao)
            {
                request.Valor = 200m;
                reservaResponse.Saldo = 800m;
            }
            else
            {
                request.Valor = 150m;
                reservaResponse.Saldo = 1150m;
            }

            await repository.CriarTransacaoAsync(Arg.Any<Transacao>());
            contaService.AtualizarReservaAsync(Arg.Any<AtualizaValorContaRequest>()).Returns(reservaResponse);
            await repository.AtualizarTransacaoAsync(Arg.Any<Transacao>());

            // Act
            var response = await service.ProcessarTransacaoAsync(request);

            // Assert
            response.ShouldBeTrue();
            await repository.Received(1).CriarTransacaoAsync(Arg.Any<Transacao>());
            await contaService.Received(1).AtualizarReservaAsync(Arg.Any<AtualizaValorContaRequest>());
            await repository.Received(1).AtualizarTransacaoAsync(Arg.Any<Transacao>());
        }

        [Theory]
        [InlineData(EOperacaoFinanceira.Credito)]
        [InlineData(EOperacaoFinanceira.Pagamento)]
        public async Task ProcessarTransacaoAsync_ComCreditoOuPagamentoValido_DeveRetornarTrue(EOperacaoFinanceira op)
        {
            // Arrange
            var request = new TransacaoRequest
            {
                IdCliente = 100,
                CodigoConta = "1234",
                TipoOperacao = op,
            };

            var saldoCreditoResponse = new AtualizaSaldoCreditoResponse
            {
                Id = 1,
                Codigo = "1234",
                DataAtualizacao = DateTime.UtcNow
            };


            if (op == EOperacaoFinanceira.Credito)
            {
                request.Valor = 200m;
                saldoCreditoResponse.SaldoCredito = 800m;
            }
            else
            {
                request.Valor = 150m;
                saldoCreditoResponse.SaldoCredito = 700m;
            }

            await repository.CriarTransacaoAsync(Arg.Any<Transacao>());
            contaService.AtualizarSaldoCreditoAsync(Arg.Any<AtualizaValorContaRequest>()).Returns(saldoCreditoResponse);
            await repository.AtualizarTransacaoAsync(Arg.Any<Transacao>());

            // Act
            var response = await service.ProcessarTransacaoAsync(request);

            // Assert
            response.ShouldBeTrue();
            await repository.Received(1).CriarTransacaoAsync(Arg.Any<Transacao>());
            await contaService.Received(1).AtualizarSaldoCreditoAsync(Arg.Any<AtualizaValorContaRequest>());
            await repository.Received(1).AtualizarTransacaoAsync(Arg.Any<Transacao>());
        }

        [Fact]
        public async Task ProcessarTransacaoAsync_ComTransferenciaValida_DeveRetornarTrue()
        {
            // Arrange
            var request = new TransacaoRequest
            {
                IdCliente = 100,
                CodigoConta = "1234",
                IdClienteDestino = 200,
                CodigoContaDestino = "5678",
                TipoOperacao = EOperacaoFinanceira.transferencia,
                Valor = 500m
            };

            var transferenciaResponse = new TransferenciaResponse
            {
                CodigoContaOrigem = "1234",
                CodigoContaDestino = "5678",
                SaldoContaOrigem = 500m,
                SaldoContaDestino = 1500m,
                DataTransferencia = DateTime.UtcNow
            };

            await repository.CriarTransacaoAsync(Arg.Any<Transacao>());
            contaService.ProcessarTransferenciaAsync(Arg.Any<TransferenciaRequest>()).Returns(transferenciaResponse);
            await repository.AtualizarTransacaoAsync(Arg.Any<Transacao>(), Arg.Any<Transferencia>());

            // Act
            var response = await service.ProcessarTransacaoAsync(request);

            // Assert
            response.ShouldBeTrue();
            await repository.Received(1).CriarTransacaoAsync(Arg.Any<Transacao>());
            await contaService.Received(1).ProcessarTransferenciaAsync(Arg.Any<TransferenciaRequest>());
            await repository.Received(1).AtualizarTransacaoAsync(Arg.Any<Transacao>(), Arg.Any<Transferencia>());
        }

        [Fact]
        public async Task ProcessarTransacaoAsync_ComRequestNulo_DeveLancarArgumentNullException()
        {
            // Act & Assert
            await Should.ThrowAsync<ArgumentNullException>(() => service.ProcessarTransacaoAsync(null));
        }

        [Fact]
        public async Task ProcessarTransacaoAsync_ComOperacaoInvalida_DeveLancarException()
        {
            // Arrange
            var request = new TransacaoRequest
            {
                IdCliente = 100,
                CodigoConta = "1234",
                TipoOperacao = (EOperacaoFinanceira)999,
                Valor = 500m
            };

            await repository.CriarTransacaoAsync(Arg.Any<Transacao>());

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(() => service.ProcessarTransacaoAsync(request));
            exception.Message.ShouldContain("Erro ao efetuar a transação");
        }

        [Fact]
        public async Task ProcessarTransacaoAsync_QuandoAtualizarSaldoFalha_DeveAtualizarStatusParaFalhou()
        {
            // Arrange
            var request = new TransacaoRequest
            {
                IdCliente = 100,
                CodigoConta = "1234",
                TipoOperacao = EOperacaoFinanceira.Deposito,
                Valor = 500m
            };

            await repository.CriarTransacaoAsync(Arg.Any<Transacao>());
            contaService.AtualizarSaldoAsync(Arg.Any<AtualizaValorContaRequest>())
                .Throws(new ArgumentException("Saldo insuficiente"));
            await repository.AtualizarTransacaoAsync(Arg.Any<Transacao>());

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(() => service.ProcessarTransacaoAsync(request));
            exception.Message.ShouldContain("Erro ao efetuar a transação");

            // Verificar que o status foi atualizado para Falhou
            await repository.Received(1).AtualizarTransacaoAsync(Arg.Is<Transacao>(t => t.StatusTransacao == EStatusTransacao.Falhou));
        }

        [Fact]
        public async Task ProcessarTransacaoAsync_QuandoAtualizarReservaFalha_DeveAtualizarStatusParaFalhou()
        {
            // Arrange
            var request = new TransacaoRequest
            {
                IdCliente = 100,
                CodigoConta = "1234",
                TipoOperacao = EOperacaoFinanceira.Aplicacao,
                Valor = 500m
            };

            await repository.CriarTransacaoAsync(Arg.Any<Transacao>());
            contaService.AtualizarReservaAsync(Arg.Any<AtualizaValorContaRequest>())
                .Throws(new ArgumentException("Saldo insuficiente"));
            await repository.AtualizarTransacaoAsync(Arg.Any<Transacao>());

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(() => service.ProcessarTransacaoAsync(request));
            exception.Message.ShouldContain("Erro ao efetuar a transação");

            // Verificar que o status foi atualizado para Falhou
            await repository.Received(1).AtualizarTransacaoAsync(Arg.Is<Transacao>(t => t.StatusTransacao == EStatusTransacao.Falhou));
        }

        [Fact]
        public async Task ProcessarTransacaoAsync_QuandoAtualizarSaldoCreditoFalha_DeveAtualizarStatusParaFalhou()
        {
            // Arrange
            var request = new TransacaoRequest
            {
                IdCliente = 100,
                CodigoConta = "1234",
                TipoOperacao = EOperacaoFinanceira.Credito,
                Valor = 500m
            };

            await repository.CriarTransacaoAsync(Arg.Any<Transacao>());
            contaService.AtualizarSaldoCreditoAsync(Arg.Any<AtualizaValorContaRequest>())
                .Throws(new ArgumentException("Saldo insuficiente"));
            await repository.AtualizarTransacaoAsync(Arg.Any<Transacao>());

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(() => service.ProcessarTransacaoAsync(request));
            exception.Message.ShouldContain("Erro ao efetuar a transação");

            // Verificar que o status foi atualizado para Falhou
            await repository.Received(1).AtualizarTransacaoAsync(Arg.Is<Transacao>(t => t.StatusTransacao == EStatusTransacao.Falhou));
        }

        [Fact]
        public async Task ProcessarTransacaoAsync_QuandoProcessarTransferenciaFalha_DeveAtualizarStatusParaFalhou()
        {
            // Arrange
            var request = new TransacaoRequest
            {
                IdCliente = 100,
                CodigoConta = "1234",
                IdClienteDestino = 200,
                CodigoContaDestino = "5678",
                TipoOperacao = EOperacaoFinanceira.transferencia,
                Valor = 500m
            };

            await repository.CriarTransacaoAsync(Arg.Any<Transacao>());
            contaService.ProcessarTransferenciaAsync(Arg.Any<TransferenciaRequest>())
                .Throws(new ArgumentException("Conta não encontrada"));
            await repository.AtualizarTransacaoAsync(Arg.Any<Transacao>());

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(() => service.ProcessarTransacaoAsync(request));
            exception.Message.ShouldContain("Erro ao efetuar a transação");

            // Verificar que o status foi atualizado para Falhou
            await repository.Received(1).AtualizarTransacaoAsync(Arg.Is<Transacao>(t => t.StatusTransacao == EStatusTransacao.Falhou));
        }

        [Fact]
        public async Task ProcessarTransacaoAsync_DeveDefinirStatusConcluida()
        {
            // Arrange
            var request = new TransacaoRequest
            {
                IdCliente = 100,
                CodigoConta = "1234",
                TipoOperacao = EOperacaoFinanceira.Deposito,
                Valor = 500m
            };

            var saldoResponse = new AtualizaSaldoResponse
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1500m,
                DataAtualizacao = DateTime.UtcNow
            };

            Transacao? transacaoAtualizada = null;
            await repository.CriarTransacaoAsync(Arg.Any<Transacao>());
            contaService.AtualizarSaldoAsync(Arg.Any<AtualizaValorContaRequest>()).Returns(saldoResponse);
            repository.AtualizarTransacaoAsync(Arg.Do<Transacao>(t => transacaoAtualizada = t));

            // Act
            await service.ProcessarTransacaoAsync(request);

            // Assert
            transacaoAtualizada.ShouldNotBeNull();
            transacaoAtualizada.StatusTransacao.ShouldBe(EStatusTransacao.Concluida);
            transacaoAtualizada.ValorFinal.ShouldBe(1500m);
        }

        [Fact]
        public async Task ProcessarTransacaoAsync_DevePreencherValorFinalComSaldoFinal()
        {
            // Arrange
            var request = new TransacaoRequest
            {
                IdCliente = 100,
                CodigoConta = "1234",
                TipoOperacao = EOperacaoFinanceira.Deposito,
                Valor = 500m
            };

            var saldoResponse = new AtualizaSaldoResponse
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 2000m,
                DataAtualizacao = DateTime.UtcNow
            };

            Transacao? transacaoAtualizada = null;
            await repository.CriarTransacaoAsync(Arg.Any<Transacao>());
            contaService.AtualizarSaldoAsync(Arg.Any<AtualizaValorContaRequest>()).Returns(saldoResponse);
            repository.AtualizarTransacaoAsync(Arg.Do<Transacao>(t => transacaoAtualizada = t));

            // Act
            await service.ProcessarTransacaoAsync(request);

            // Assert
            transacaoAtualizada.ShouldNotBeNull();
            transacaoAtualizada.ValorFinal.ShouldBe(2000m);
        }

        [Fact]
        public async Task ProcessarTransacaoAsync_QuandoRepositoryErroAoAtualizar_DeveLancarException()
        {
            // Arrange
            var request = new TransacaoRequest
            {
                IdCliente = 100,
                CodigoConta = "1234",
                TipoOperacao = EOperacaoFinanceira.Deposito,
                Valor = 500m
            };

            var saldoResponse = new AtualizaSaldoResponse
            {
                Id = 1,
                Codigo = "1234",
                Saldo = 1500m,
                DataAtualizacao = DateTime.UtcNow
            };

            await repository.CriarTransacaoAsync(Arg.Any<Transacao>());
            contaService.AtualizarSaldoAsync(Arg.Any<AtualizaValorContaRequest>()).Returns(saldoResponse);
            repository.AtualizarTransacaoAsync(Arg.Any<Transacao>())
                .Throws(new Exception("Erro na base de dados"));

            // Act & Assert
            var exception = await Should.ThrowAsync<Exception>(() => service.ProcessarTransacaoAsync(request));
            exception.Message.ShouldContain("Erro inesperado ao efetuar a transação");
        }
    }
}
