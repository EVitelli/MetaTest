using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Services
{
    public class AuthService : IAuthService
    {
        public string GenerateToken(TokenRequest usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("f0035a43-8a07-40a8-bb27-603230851c15");
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim("Id", usuario.Id.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
            claims.AddClaim(new Claim(ClaimTypes.Role, usuario.Role));


            var token = tokenHandler.CreateJwtSecurityToken(
                issuer: "PagueVeloz",
                audience: "Webapi",
                subject: claims,
                notBefore: null,
                expires: DateTime.UtcNow.AddDays(1),
                issuedAt: DateTime.UtcNow,
                signingCredentials: credentials
            );

            return tokenHandler.WriteToken(token);
        }
    }
}
