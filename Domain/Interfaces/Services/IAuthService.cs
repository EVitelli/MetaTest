using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IAuthService
    {
        string GenerateToken(TokenRequest usuario);
    }
}
