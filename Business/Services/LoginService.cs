using Domain.Interfaces.Services;
using Domain.Models;

namespace Business.Services
{
    public class LoginService(IAuthService authService, IUsuarioService usuarioService) : ILoginService
    {
        public async Task<string?> LoginAsync(LoginRequest usuario)
        {
            UsuarioAuthInfoResponse? authInfo = await usuarioService.BuscarAuthInfoAsync(new UsuarioAuthInfoRequest()
            {
                Email = usuario.Email,
                Hash = usuario.Senha
            });

            if (authInfo is null)
                return null;

            return authService.GenerateToken(new TokenRequest
            {
                Email = authInfo.Email,
                Tipo = authInfo.Tipo
            });
        }
    }
}
