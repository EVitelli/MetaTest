using Domain.Models;

namespace Domain.Interfaces.Services
{
    public  interface ILoginService
    {
        Task<string?> LoginAsync(LoginRequest request);
    }
}
