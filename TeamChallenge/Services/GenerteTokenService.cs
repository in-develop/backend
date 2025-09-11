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
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        List<Claim> claims = CreateClaims(user, roles, cartId);

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

        return tokenHandler.WriteToken(token);
    }

    private static List<Claim> CreateClaims(UserEntity user, IList<string> roles, int cartId)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("CartId", cartId.ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}
