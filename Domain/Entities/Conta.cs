using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Conta : IEntity, IDelete
    {
        public uint Id { get; set; }
        public uint IdUsuarioCliente { get; set; }
        public uint IdUsuarioGerente { get; set; }
        public string Codigo { get; set; }
        public decimal Saldo { get; set; }
        public decimal Reservado { get; set; }
        public decimal LimiteCredito { get; set; }
        public decimal SaldoCredito { get; set; }
        public DateTime CriadoEm { get ;set; } = DateTime.Now;
        public DateTime AtualizadoEm { get ;set; } = DateTime.Now;
        public DateTime? DeletadoEm { get ; set ; }
        public EStatus Status { get ; set ; }
    }
}
