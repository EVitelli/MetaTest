using Domain.Enums;

namespace Domain.Models
{
    public class UsuarioRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ETipoUsuario Tipo { get; set; }
        public string Cpf { get; set; } = string.Empty;
    }

    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
