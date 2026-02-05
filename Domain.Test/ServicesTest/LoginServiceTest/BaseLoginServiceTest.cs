using Business.Services;
using Domain.Interfaces.Services;
using NSubstitute;

namespace Domain.Test.ServicesTest.LoginServiceTest
{
    public class BaseLoginServiceTest
    {
        public readonly LoginService service;
        public readonly IAuthService authService = Substitute.For<IAuthService>();
        public readonly IUsuarioService usuarioService = Substitute.For<IUsuarioService>();

        public BaseLoginServiceTest()
        {
            service = new LoginService(authService, usuarioService);
        }
    }
}
