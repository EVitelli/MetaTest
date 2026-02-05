using Business.Services;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Test.ServicesTest.TransacaoServiceTest
{
    public class BaseTransacaoServiceTest
    {
        public readonly TransacaoService service;
        public readonly ITransacaoRepository repository = Substitute.For<ITransacaoRepository>();
        public readonly IContaService contaService = Substitute.For<IContaService>();

        public BaseTransacaoServiceTest()
        {
            service = new TransacaoService(repository, contaService);
        }
    }
}
