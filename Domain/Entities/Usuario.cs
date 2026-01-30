using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Usuario : IEntity
    {
        public int Id { get; set; }
        public EStatus Status { get; set; }
        public required string Nome { get; set; }
        public ETipoUsuario Tipo { get; set; }
        public required string Cpf { get; set; }
        public required string Email { get; set; }
        public string Hash { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
    }
}
