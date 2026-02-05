using Business.Services;
using Domain.Interfaces.Repositories;
using NSubstitute;

namespace Domain.Test.ServicesTest.ContaServiceTest
{
    public class BaseContaServiceTest
    {
        public readonly ContaService service;
        public readonly IContaRepository repository = Substitute.For<IContaRepository>();

        public BaseContaServiceTest()
        {
            service = new ContaService(repository);
        }
    }
}
