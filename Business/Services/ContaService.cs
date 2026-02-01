using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace Business.Services
{
    public class ContaService(IContaRepository repository, IUsuarioService usuarioService) : IContaService

    {
        public Task<AtualizaLimiteCreditoResponse> AtualizarLimiteCreditoAsync(ContaRequest conta)
        {
            throw new NotImplementedException();
        }

        public async Task<AtualizaReservaResponse> AtualizarReservaAsync(AtualizaValorContaRequest request)
        {
            try
            {
                Conta? conta = await repository.BuscarContaPorCodigoAsync(request.CodigoConta);

                ValidaConta(conta);

                if (request.Operacao != EOperacaoFinanceira.Resgate &&
                   request.Operacao != EOperacaoFinanceira.Aplicacao)
                    throw new ArgumentException("Operação incorreta para reserva.");

                if (request.Operacao == EOperacaoFinanceira.Aplicacao)
                {
                    if (conta.Saldo - request.Valor < 0)
                        throw new ArgumentException("Saldo insuficiente para a operação.");

                    conta.Saldo -= request.Valor;
                    conta.Reservado += request.Valor;
                }
                else if (request.Operacao == EOperacaoFinanceira.Resgate)
                {
                    if (conta.Reservado - request.Valor < 0)
                        throw new ArgumentException("Valor reservado insuficiente para a operação.");

                    conta.Saldo += request.Valor;
                    conta.Reservado -= request.Valor;
                }

                await repository.AtualizarContaAsync(conta);

                return new AtualizaReservaResponse
                {
                    Id = conta.Id,
                    Codigo = conta.Codigo,
                    Saldo = conta.Saldo,
                    Reservado = conta.Reservado,
                    DataAtualizacao = conta.AtualizadoEm
                };

            }
            catch (ArgumentException ae)
            {
                throw new ArgumentException(message: "Erro ao atualizar sreserva da conta.", ae);
            }
            catch (Exception e)
            {
                throw new Exception(message: "Erro inesperado ao atualizar reserva da conta.", e);
            }
        }

        public async Task<AtualizaSaldoResponse> AtualizarSaldoAsync(AtualizaValorContaRequest request)
        {
            try
            {
                Conta? conta = await repository.BuscarContaPorCodigoAsync(request.CodigoConta);

                ValidaConta(conta);

                decimal novoSaldo =
                    request.Operacao == EOperacaoFinanceira.Deposito ?
                        conta.Saldo + request.Valor
                    : request.Operacao == EOperacaoFinanceira.Debito ?
                        conta.Saldo - request.Valor
                    : throw new ArgumentException("Operação incorreta");

                if (novoSaldo < 0)
                    throw new ArgumentException("Saldo insuficiente para a operação.");

                conta.Saldo = novoSaldo;

                await repository.AtualizarContaAsync(conta);

                return new AtualizaSaldoResponse
                {
                    Id = conta.Id,
                    Codigo = conta.Codigo,
                    Saldo = conta.Saldo,
                    DataAtualizacao = conta.AtualizadoEm
                };
            }
            catch (ArgumentException ae)
            {
                throw new ArgumentException(message: "Erro ao atualizar saldo da conta.", ae);
            }
            catch (Exception e)
            {
                throw new Exception(message: "Erro inesperado ao atualizar saldo da conta.", e);
            }
        }

        public async Task<AtualizaSaldoCreditoResponse> AtualizarSaldoCreditoAsync(AtualizaValorContaRequest request)
        {
            try
            {
                Conta? conta = await repository.BuscarContaPorCodigoAsync(request.CodigoConta);

                ValidaConta(conta);

                decimal novoSaldo =
                    request.Operacao == EOperacaoFinanceira.Pagamento ?
                        conta.SaldoCredito + request.Valor
                    : request.Operacao == EOperacaoFinanceira.Credito ?
                        conta.SaldoCredito - request.Valor
                    : throw new ArgumentException("Operação incorreta para crédito.");

                if (novoSaldo < 0)
                    throw new ArgumentException("Saldo insuficiente para a operação.");

                if (novoSaldo > conta.LimiteCredito)
                    throw new ArgumentException("Pagamento superior ao devido.");

                conta.SaldoCredito = novoSaldo;

                await repository.AtualizarContaAsync(conta);

                return new AtualizaSaldoCreditoResponse
                {
                    Id = conta.Id,
                    Codigo = conta.Codigo,
                    SaldoCredito = conta.SaldoCredito,
                    DataAtualizacao = conta.AtualizadoEm
                };
            }
            catch (ArgumentException ae)
            {
                throw new ArgumentException(message: "Erro ao atualizar saldo crédito da conta.", ae);
            }
            catch (Exception e)
            {
                throw new Exception(message: "Erro inesperado ao atualizar saldo crédito da conta.", e);
            }
        }

        public async Task<GetContaResponse?> BuscarContaPorCodigoAsync(string codigoConta)
        {
            Conta? contaDb = await repository.BuscarContaPorCodigoAsync(codigoConta);

            if (contaDb is null)
                return null;

            return new GetContaResponse
            {
                Id = contaDb.Id,
                ClienteId = contaDb.IdUsuarioCliente,
                GerenteId = contaDb.IdUsuarioGerente,
                Codigo = contaDb.Codigo,
                Saldo = contaDb.Saldo,
                Reservado = contaDb.Reservado,
                LimiteCredito = contaDb.LimiteCredito,
                SaldoCredito = contaDb.SaldoCredito,
                DataCriacao = contaDb.CriadoEm,
                CriadoEm = contaDb.CriadoEm,
                AtualizadoEm = contaDb.AtualizadoEm,
                DeletadoEm = contaDb.DeletadoEm,
                Status = contaDb.Status
            };
        }

        public async Task<PostContaResponse> CriarContaAsync(ContaRequest conta)
        {
            ArgumentNullException.ThrowIfNull(conta);
            try
            {
                await ValidaEnvolvidosAsync(conta);

                Conta novaConta = new()
                {
                    IdUsuarioCliente = conta.ClienteId,
                    IdUsuarioGerente = conta.GerenteId,
                    LimiteCredito = conta.LimiteCredito,
                    Codigo = CriarCodigo(),
                    Saldo = 0,
                    Reservado = 0,
                    SaldoCredito = conta.LimiteCredito,
                    CriadoEm = DateTime.UtcNow,
                    AtualizadoEm = DateTime.UtcNow,
                    Status = EStatus.Ativo
                };

                novaConta = await repository.CriarContaAsync(novaConta);

                return new PostContaResponse
                {
                    Id = novaConta.Id,
                    ClienteId = novaConta.IdUsuarioCliente,
                    GerenteId = novaConta.IdUsuarioGerente,
                    Codigo = novaConta.Codigo,
                    LimiteCredito = novaConta.LimiteCredito,
                    DataCriacao = novaConta.CriadoEm
                };
            }
            catch (ArgumentException ae)
            {
                throw new ArgumentException(message: "Erro ao criar um nova conta.", ae);
            }
            catch (Exception e)
            {
                throw new Exception(message: "Erro inesperado ao criar um nova conta.", e);
            }
        }

        public Task<DeleteContaResponse> DeletarContaAsync(uint id)
        {
            throw new NotImplementedException();
        }

        public async Task<TransferenciaResponse> ProcessarTransferenciaAsync(TransferenciaRequest request)
        {

            try
            {
                Conta? contaOrigem = await repository.BuscarContaPorCodigoAsync(request.CodigoContaOrigem);
                Conta? contaDestino = await repository.BuscarContaPorCodigoAsync(request.CodigoContaDestino);

                try
                {
                    ValidaConta(contaOrigem);
                }
                catch (ArgumentException aex)
                {
                    throw new ArgumentException("Conta de origem inválida para transferência.", aex);
                }

                try
                {
                    ValidaConta(contaDestino);
                }
                catch (ArgumentException aex)
                {
                    throw new ArgumentException("Conta de destino inválida para transferência.", aex);
                }

                if (contaOrigem.Saldo < request.Valor)
                    throw new ArgumentException("Saldo insuficiente para a transferência.");

                contaOrigem.Saldo -= request.Valor;
                contaDestino.Saldo += request.Valor;

                await repository.AtualizarContaAsync(contaOrigem);
                await repository.AtualizarContaAsync(contaDestino);

                return new TransferenciaResponse
                {
                    CodigoContaDestino = contaDestino.Codigo,
                    CodigoContaOrigem = contaOrigem.Codigo,
                    SaldoContaOrigem = contaOrigem.Saldo,
                    SaldoContaDestino = contaDestino.Saldo,
                    DataTransferencia = DateTime.UtcNow
                };
            }
            catch (ArgumentException ae)
            {
                throw new ArgumentException(message: "Erro ao processar transferência.", ae);
            }
            catch (Exception e)
            {
                throw new Exception(message: "Erro inesperado ao processar transferência.", e);
            }
        }

        /* Gera um código aleatório para a conta no formato 0000
         * - Pode ser criado em outro lugar, adicionei aqui para facilitar o desenvolvimento.
         * - Adicionei este método pra ter um exemplo simples de lógica de negócio próximo ao real 
         * pois imagino que a criação de código possa ser mais complexa em um cenário real
         * - A lógica é simples, precisei gerar dois números aleatórios pois queria que o primeiro
         * digito pudesse ser 0, o que não é possível se eu gerar um número entre 0 e 9999. Os 
         * outros digitos são gerados entre 1000 e 9999 e depois eu removo o primeiro dígito, 
         * assim também consigo ter zeros no meio do código.
         * - Não me aprofundei em verificar se o código já existe pois a quantidade de dados será baixa 
         * e a probabilidade de duplicação também, mas em um cenário real existe a verificação de duplicidade
         * já que este valor deve ser único.
         */
        private static string CriarCodigo()
        {
            Random random = new();

            int n1 = random.Next(0, 9), n2 = random.Next(1000, 9999);

            return n1.ToString() + n2.ToString().Substring(1);
        }

        private async Task ValidaEnvolvidosAsync(ContaRequest conta)
        {
            if (conta.ClienteId == conta.GerenteId)
                throw new ArgumentException("O cliente e o gerente não podem ser a mesma pessoa.");

            UsuarioResponse? cliente = await usuarioService.BuscarTodasInfoUsuarioAsync(conta.ClienteId);
            if (cliente is null || cliente.Status == EStatus.Inativo || cliente.Tipo != ETipoUsuario.Cliente)
                throw new ArgumentException("O cliente não foi encontrado ou foi removido.");

            UsuarioResponse? gerente = await usuarioService.BuscarTodasInfoUsuarioAsync(conta.GerenteId);
            if (gerente is null || gerente.Status == EStatus.Inativo || gerente.Tipo != ETipoUsuario.Gerente)
                throw new ArgumentException("O gerente não foi encontrado ou foi removido.");
        }

        private static void ValidaConta(Conta? conta)
        {
            if (conta is null || conta.Status == EStatus.Inativo)
                throw new ArgumentException("A conta não foi encontrada ou está inativa.");
        }
    }
}
