using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Services;

public class GenerateTokenService(IConfiguration configuration) : IGenerateToken
{
    public string GenerateToken(UserEntity? user, IList<string> roles, bool rememberMe)
    {
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        var claims = CreateClaims(user, roles);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var time = double.Parse(rememberMe ? configuration["Jwt:RememberMe"]! : configuration["Jwt:Expires"]!);

        var tokenExpiration = DateTime.UtcNow.Add(TimeSpan.FromMinutes(time));
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: tokenExpiration,
            signingCredentials: signingCredentials);

        var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenHandler;
    }

    private static List<Claim> CreateClaims(UserEntity? user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user!.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
}