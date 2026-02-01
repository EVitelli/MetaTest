using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Transacao : IEntity
    {
        public uint Id { get; set; }
        public uint IdUsuarioCliente { get; set; }
        public string CodigoConta { get; set; }
        public EOperacaoFinanceira Tipo { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorFinal { get; set; }
        public EStatusTransacao StatusTransacao { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
    }
}
