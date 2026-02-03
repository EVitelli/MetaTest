using Domain.Enums;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class CriarUsuarioRequest
    {
        public required string Nome { get; set; }
        public ETipoUsuario Tipo { get; set; }
        public required string Cpf { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
        [JsonIgnore]
        public string Operador { get; set; } = string.Empty;
    }

    public class CriarUsuarioResponse
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

    public class DeletarUsuarioRequest
    {
        public uint Id { get; set; }
        public required string Operador { get; set; }
    }

    public class DeletarUsuarioResponse
    {
        public uint Id { get; set; }
        public required string Email { get; set; }
        public DateTime DataDelecao { get; set; }
    }

    public class UsuarioAuthInfoRequest
    {
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }

    public class UsuarioAuthInfoResponse
    {
        public required uint Id { get; set; }
        public required string Email { get; set; }
        public ETipoUsuario Tipo { get; set; }
        public required byte[] Salt { get; set; }
        public required string Hash { get; set; }
    }
}
