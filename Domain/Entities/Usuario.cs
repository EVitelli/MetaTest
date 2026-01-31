using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Usuario : IEntity, IDelete
    {
        public uint Id { get; set; }
        public EStatus Status { get; set; }
        public required string Nome { get; set; }
        public ETipoUsuario Tipo { get; set; }
        public required string Cpf { get; set; }
        public required string Email { get; set; }
        public string? Hash { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.Now;
        public DateTime AtualizadoEm { get; set; } = DateTime.Now;
        public DateTime? DeletadoEm { get; set; }
    }
}
