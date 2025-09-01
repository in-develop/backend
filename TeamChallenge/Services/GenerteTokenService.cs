using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeamChallenge.Models.Entities;
using TeamChallenge.Services;

public class GenerateTokenService: IGenerateToken
{
    private readonly IConfiguration _configuration;

    public GenerateTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (string, DateTime) GenerateToken(UserEntity user, IList<string> roles, bool remebmerMe)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        DateTime tokenExpiration;
        if (remebmerMe)
        {
            tokenExpiration = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration["Jwt:RememberMe"]!));
        }
        else
        {
            tokenExpiration = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration["Jwt:Expires"]!));
        }
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: tokenExpiration,
            signingCredentials: creds);

        return (tokenHandler.WriteToken(token), tokenExpiration);
    }
}
