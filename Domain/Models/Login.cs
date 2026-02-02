using Domain.Enums;

namespace Domain.Models
{
    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }

    public class LoginResponse
    {
        public required string Token { get; set; }
    }

    public class TokenRequest
    {
        public required string Email { get; set; }
        public ETipoUsuario Tipo { get; set; }
        public string Role { get => Tipo.ToString(); }
    }
}
