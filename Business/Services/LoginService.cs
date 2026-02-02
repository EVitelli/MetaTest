using Domain.Auth;
using Domain.Interfaces.Services;
using Domain.Models;

namespace Business.Services
{
    public class LoginService(IAuthService authService, IUsuarioService usuarioService) : ILoginService
    {
        public async Task<string?> LoginAsync(LoginRequest usuario)
        {
            UsuarioAuthInfoResponse? authInfo = await usuarioService.BuscarAuthInfoAsync(usuario.Email);

            if (authInfo is null || !PasswordHasher.VerifyPassword(usuario.Senha, authInfo.Hash, authInfo.Salt))
                return null;

            return authService.GenerateToken(new TokenRequest
            {
                Email = authInfo.Email,
                Tipo = authInfo.Tipo
            });
        }
    }
}
