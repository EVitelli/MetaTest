using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace Business.Services
{
    /* - Foi adicionado a referencia a usuarioRepository porém em um cenario com microsserviços
    * isso estária em outro serviço
    */
    public class ContaService(IContaRepository repository, IUsuarioService usuarioService) : IContaService

    {
        public async Task<PostContaResponse> CriarConta(ContaRequest conta)
        {
            ArgumentNullException.ThrowIfNull(conta);
            try
            {
                await ValidaEnvolvidos(conta);

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

                novaConta = await repository.CriarConta(novaConta);

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
                throw new Exception(message: "Erro ao criar um nova conta.", ae);
            }
            catch (Exception e)
            {
                throw new Exception(message: "Erro inesperado ao criar um nova conta.", e);
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
        private string CriarCodigo()
        {
            Random random = new();

            int n1 = random.Next(0, 9), n2 = random.Next(1000, 9999);

            return n1.ToString() + n2.ToString().Substring(1);
        }

        private async Task ValidaEnvolvidos(ContaRequest conta)
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
    }
}
