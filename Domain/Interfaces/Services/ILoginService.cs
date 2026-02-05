using Domain.Models;

namespace Domain.Interfaces.Services
{
    public  interface ILoginService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}
