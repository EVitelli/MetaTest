using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ContaRequest
    {
        public uint ClienteId { get; set; }
        public uint GerenteId { get; set; }
        public decimal LimiteCredito { get; set; }
    }

    public class PostContaResponse
    {
        public uint Id { get; set; }
        public uint ClienteId { get; set; }
        public uint GerenteId { get; set; }
        public string Codigo { get; set; }
        public decimal LimiteCredito { get; set; }
        public DateTime DataCriacao { get; set; }
    }

    public class GetContaResponse
    {
        public uint Id { get; set; }
        public required uint ClienteId { get; set; }
        public required uint GerenteId { get; set; }
        public string Codigo { get; set; }
        public decimal Saldo { get; set; }
        public decimal Reservado { get; set; }
        public decimal LimiteCredito { get; set; }
        public decimal SaldoCredito { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.Now;
        public DateTime AtualizadoEm { get; set; } = DateTime.Now;
        public DateTime? DeletadoEm { get; set; }
        public EStatus Status { get; set; }
    }

    public class DeleteContaResponse
    {
        public uint Id { get; set; }
        public required string Codigo { get; set; }
        public DateTime DataDelecao { get; set; }
    }

    public class AtualizaLimiteCreditoResponse
    {
        public uint Id { get; set; }
        public required string Codigo { get; set; }
        public decimal LimiteCredito { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizaReservaResponse
    {
        public uint Id { get; set; }
        public required string Codigo { get; set; }
        public decimal Reservado { get; set; }
        public decimal Saldo { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizaSaldoResponse
    {
        public uint Id { get; set; }
        public required string Codigo { get; set; }
        public decimal Saldo { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizaValorContaRequest(string codigoConta, decimal valor, EOperacaoFinanceira operacao)
    {
        public string CodigoConta { get; set; } = codigoConta;
        public decimal Valor { get; set; } = valor;
        public EOperacaoFinanceira Operacao { get; set; } = operacao;    
    }

    public class AtualizaSaldoCreditoResponse
    {
        public uint Id { get; set; }
        public required string Codigo { get; set; }
        public decimal SaldoCredito { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }

    public class TransferenciaRequest
    {
        public required string CodigoContaOrigem { get; set; }
        public required string CodigoContaDestino { get; set; }
        public decimal Valor { get; set; }
    }

    public class TransferenciaResponse
    {
        public required string CodigoContaOrigem { get; set; }
        public required string CodigoContaDestino { get; set; }
        public decimal SaldoContaOrigem { get; set; }
        public decimal SaldoContaDestino { get; set; }
        public DateTime DataTransferencia { get; set; }
    }

}
