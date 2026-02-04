using Business.Services;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure;
using Infrastructure.Repositories;

namespace WebApi.Config
{
    public static class DependencyInjection
    {
        public static void Inject(IServiceCollection service)
        {
            service.AddDbContext<DatabaseContext>();
            service.AddTransient<IAuthService, AuthService>();
            service.AddTransient<ILoginService, LoginService>();
            service.AddTransient<IUsuarioRepository, UsuarioRepository>();
            service.AddTransient<IUsuarioService, UsuarioService>();
            service.AddTransient<IContaRepository, ContaRepository>();
            service.AddTransient<IContaService, ContaService>();
            service.AddScoped<ITransacaoRepository, TransacaoRepository>();
            service.AddScoped<ITransacaoService, TransacaoService>();
        }
    }
}