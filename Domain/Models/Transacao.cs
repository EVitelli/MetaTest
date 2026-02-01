using Domain.Enums;

namespace Domain.Models
{
    public class TransacaoRequest
    {
        public uint IdCliente{ get; set; }
        public string CodigoConta { get; set; }
        public uint IdClienteDestino { get; set; }
        public string CodigoContaDestino { get; set; } = string.Empty;
        public EOperacaoFinanceira TipoOperacao { get; set; }
        public decimal Valor { get; set; }
    }
}
