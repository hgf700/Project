using Microsoft.IdentityModel.Tokens;
using ProjectBackend.Models.ReleatedToSocial;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectBackend.Services;

public class JwtService
{
    string JWT_SECRET = Environment.GetEnvironmentVariable("JWT_SECRET");

    public string GenerateToken(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(JWT_SECRET));

        var creds = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

