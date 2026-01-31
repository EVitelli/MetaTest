using Domain.Enums;

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
}
