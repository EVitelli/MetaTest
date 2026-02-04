using Business.Services;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using NSubstitute;

namespace Domain.Test.ServicesTest.UsuarioServiceTest
{
    public class BaseUsuarioServiceTest
    {
        public readonly UsuarioService service;
        public readonly IUsuarioRepository repository = Substitute.For<IUsuarioRepository>();
        public readonly IContaService contaService = Substitute.For<IContaService>();

        public BaseUsuarioServiceTest()
        {
            service = new UsuarioService(repository, contaService);
        }
    }
}
