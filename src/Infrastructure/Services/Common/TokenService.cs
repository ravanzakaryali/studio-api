using Microsoft.Extensions.Configuration;
using Space.Application.Abstractions;
using Space.Application.DTOs;

namespace Space.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateRefreshToken()
    {
        byte[] number = new byte[32];
        using RandomNumberGenerator random = RandomNumberGenerator.Create();
        random.GetBytes(number);
        return Convert.ToBase64String(number);
    }
    public Token GenerateToken(User user, TimeSpan time, IList<string>? roles = null)
    {
        DateTime expires = DateTime.UtcNow.Add(time);
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Exp, expires.ToString())
        };
        if (roles != null)
        {
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        }

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:SecurityKey").Value!));

        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken securityToken = new(
            issuer: _configuration.GetSection("Jwt:Issuer").Value,
            audience: _configuration.GetSection("Jwt:Audience").Value,
            claims: claims,
        expires: expires,
            signingCredentials: credentials);

        return new Token()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(securityToken),
            Expires = expires,
        };
    }
    public string GenerateVerificationCode(int length = 6)
    {
        //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        const string chars = "0123456789";
        var random = new Random();

        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        TokenValidationParameters tokenValidationParameters = new()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"])),
            ValidAudience = _configuration["Jwt:Audience"],
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidateLifetime = false
        };

        JwtSecurityTokenHandler tokenHandler = new();
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        return principal;
    }
}
