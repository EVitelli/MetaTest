namespace Domain.Entities
{
    public class Transferencia
    {
        public uint IdTransacao { get; set; }
        public uint IdUsuarioClienteDestino { get; set; }
        public string CodigoContaDestino { get; set; }
        public decimal ValorFinal { get; set; }
    }
}
