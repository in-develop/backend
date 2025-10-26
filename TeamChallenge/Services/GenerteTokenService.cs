using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TeamChallenge.Models.Entities;
using TeamChallenge.StaticData;

namespace TeamChallenge.Services;

public class GenerateTokenService(IConfiguration configuration) : IGenerateToken
{
    public string GenerateToken(UserEntity user, IList<string> roles, bool remebmerMe, int cartId)
    {
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        List<Claim> claims = CreateClaims(user, roles, cartId);
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        double time;
        if (remebmerMe)
        {
            time = double.Parse(configuration["Jwt:RememberMe"]!);
        }
        else
        {
            time = double.Parse(configuration["Jwt:Expires"]!);
        }

        var tokenExpiration = DateTime.UtcNow.Add(TimeSpan.FromMinutes(time));
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: tokenExpiration,
            signingCredentials: creds);

        var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenHandler;
    }

    private static List<Claim> CreateClaims(UserEntity user, IList<string> roles, int cartId)
    {
        var claims = new List<Claim>
        {
            new Claim(CustomClaimTypes.CartId, cartId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}