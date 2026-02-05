using Business.Services;
using Domain.Interfaces.Services;
using NSubstitute;

namespace Domain.Test.ServicesTest.AuthServiceTest
{
    public class BaseAuthServiceTest
    {
        public readonly AuthService service;

        public BaseAuthServiceTest()
        {
            service = new AuthService();
        }
    }
}
