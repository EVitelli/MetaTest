using Domain.Auth;
using Domain.Interfaces.Services;
using Domain.Models;

namespace Business.Services
{
    public class LoginService(IAuthService authService, IUsuarioService usuarioService) : ILoginService
    {
        public async Task<string?> LoginAsync(LoginRequest request)
        {
            UsuarioAuthInfoResponse? usuario = await usuarioService.BuscarAuthInfoAsync(request.Email);

            if (usuario is null || !PasswordHasher.VerifyPassword(request.Senha, usuario.Hash, usuario.Salt))
                return null;

            return authService.GenerateToken(new TokenRequest
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Tipo = usuario.Tipo
            });
        }
    }
}
