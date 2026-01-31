using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IContaService
    {
        Task<PostContaResponse> CriarConta(ContaRequest conta);
    }
}
