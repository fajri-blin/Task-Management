using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task_Management.Contract.Handler;

namespace Task_Management.Utilities.Handler;

public class TokenHandlers : ITokenHandlers
{
    private readonly IConfiguration _configuration;

    public TokenHandlers(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTService:Key"]));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokenOptions = new JwtSecurityToken(issuer: _configuration["JWTService:Issuer"],
                                                audience: _configuration["JWTService:Audience"],
                                                claims: claims,
                                                expires: DateTime.Now.AddMinutes(10),
                                                signingCredentials: signinCredentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return tokenString;
    }
}
