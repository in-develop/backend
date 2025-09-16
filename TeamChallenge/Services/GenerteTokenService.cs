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

    public string GenerateToken(UserEntity user, IList<string> roles, bool remebmerMe, int cartId)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        List<Claim> claims = CreateClaims(user, roles, cartId);
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        double time;
        if (remebmerMe)
        {
            time = double.Parse(_configuration["Jwt:RememberMe"]!);
        }
        else
        {
            time = double.Parse(_configuration["Jwt:Expires"]!);
        }

        var tokenExpiration = DateTime.UtcNow.Add(TimeSpan.FromMinutes(time));
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
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
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Actor, cartId.ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}
