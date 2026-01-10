using Microsoft.IdentityModel.Tokens;
using ProjectBackend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectBackend.Services;

public class JwtService
{
    //string JWT_SECRET = Environment.GetEnvironmentVariable("JWT_SECRET");
    private readonly string _secret;

    public JwtService(IConfiguration config)
    {
        _secret = config["JWT_SECRET"];
    }

    public string GenerateToken(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_secret));

        var creds = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

