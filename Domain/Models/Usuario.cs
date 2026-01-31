using Domain.Enums;

namespace Domain.Models
{
    public class UsuarioRequest
    {
        public required string Nome { get; set; }
        public ETipoUsuario Tipo { get; set; }
        public required string Cpf { get; set; }
        public required string Email { get; set; }
    }

    public class PostUsuarioResponse
    {
        public uint Id { get; set; }
        public required string Email { get; set; }
        public DateTime DataCriacao { get; set; }
    }

    public class GetUsuarioResponse
    {
        public uint Id { get; set; }
        public required string Email { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public required string Nome { get; set; }
        public required string Cpf { get; set; }
    }

    public class UsuarioResponse
    {
        public uint Id { get; set; }
        public EStatus Status { get; set; }
        public required string Nome { get; set; }
        public ETipoUsuario Tipo { get; set; }
        public required string Cpf { get; set; }
        public required string Email { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;
        public DateTime? DataDelecao { get; set; }
    }

    public class DeleteUsuarioResponse
    {
        public uint Id { get; set; }
        public required string Email { get; set; }
        public DateTime DataDelecao { get; set; }
    }
}
