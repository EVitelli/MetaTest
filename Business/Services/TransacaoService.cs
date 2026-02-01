using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace Business.Services
{
    public class TransacaoService(ITransacaoRepository repository, IContaService contaService) : ITransacaoService
    {
        public async Task<bool> ProcessarTransacaoAsync(TransacaoRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            Transacao transacao = new()
            {
                IdUsuarioCliente = request.IdCliente,
                CodigoConta = request.CodigoConta,
                Tipo = request.TipoOperacao,
                Valor = request.Valor
            };

            try
            {
                await repository.CriarTransacaoAsync(transacao);

                AtualizaValorContaRequest contaRequest = new(transacao.CodigoConta, transacao.Valor, transacao.Tipo);

                switch (transacao.Tipo)
                {
                    case EOperacaoFinanceira.Deposito:
                    case EOperacaoFinanceira.Debito:
                        AtualizaSaldoResponse saldoResponse = await contaService.AtualizarSaldoAsync(contaRequest);

                        transacao.ValorFinal = saldoResponse.Saldo;
                        transacao.StatusTransacao = EStatusTransacao.Concluida;
                        await repository.AtualizarTransacaoAsync(transacao);

                        return true;
                    case EOperacaoFinanceira.Aplicacao:
                    case EOperacaoFinanceira.Resgate:
                        AtualizaReservaResponse reservaResponse = await contaService.AtualizarReservaAsync(contaRequest);

                        transacao.ValorFinal = reservaResponse.Reservado;
                        transacao.StatusTransacao = EStatusTransacao.Concluida;
                        await repository.AtualizarTransacaoAsync(transacao);

                        return true;
                    case EOperacaoFinanceira.Credito:
                    case EOperacaoFinanceira.Pagamento:
                        AtualizaSaldoCreditoResponse saldoCreditoResponse = await contaService.AtualizarSaldoCreditoAsync(contaRequest);

                        transacao.ValorFinal = saldoCreditoResponse.SaldoCredito;
                        transacao.StatusTransacao = EStatusTransacao.Concluida;
                        await repository.AtualizarTransacaoAsync(transacao);

                        return true;
                    case EOperacaoFinanceira.transferencia:
                        TransferenciaRequest transferenciaRequest = new()
                        {
                            CodigoContaOrigem = transacao.CodigoConta,
                            CodigoContaDestino = request.CodigoContaDestino,
                            Valor = transacao.Valor,
                        };

                        TransferenciaResponse transferenciaResponse = await contaService.ProcessarTransferenciaAsync(transferenciaRequest);

                        Transferencia transferencia = new()
                        {
                            IdTransacao = transacao.Id,
                            IdUsuarioClienteDestino = request.IdClienteDestino,
                            CodigoContaDestino = request.CodigoContaDestino,
                            ValorFinal = transferenciaResponse.SaldoContaDestino
                        };

                        transacao.ValorFinal = transferenciaResponse.SaldoContaOrigem;
                        transacao.StatusTransacao = EStatusTransacao.Concluida;
                        await repository.AtualizarTransacaoAsync(transacao, transferencia);

                        return true;
                    default:
                        throw new ArgumentException("Operação inválida");
                }
            }
            catch (ArgumentException aex)
            {
                transacao.StatusTransacao = EStatusTransacao.Falhou;
                await repository.AtualizarTransacaoAsync(transacao);

                throw new Exception("Erro ao efetuar a transação", aex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao efetuar a transação", ex);
            }
        }
    }
}
