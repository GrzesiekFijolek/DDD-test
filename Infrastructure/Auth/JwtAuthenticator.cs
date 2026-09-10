using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Security;
using Application.Security.Response;
using Domain.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

internal sealed class JwtAuthenticator : IAuthenticator
{
    private readonly IClock _clock;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly TimeSpan _expiry;
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

    public JwtAuthenticator(IOptions<AuthOptions> authOptions, IClock clock)
    {
        _clock = clock;
        _issuer = authOptions.Value.Issuer;
        _audience = authOptions.Value.Audience;
        _expiry = authOptions.Value.Expiry;
        _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Value.SigningKey)),
            SecurityAlgorithms.HmacSha256);
    }
    
    public TokenResponse CreateToken(long userId, string role)
    {
        var now = _clock.Current();

        var expires = now.Add(_expiry);
        
        var claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Sub, userId.ToString()),
            new (JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new (ClaimTypes.Role, userId.ToString(), role)
        };

        var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires, _signingCredentials);
        var accesToken = _jwtSecurityTokenHandler.WriteToken(jwt);

        return new TokenResponse() { AccessToken = accesToken };
    }
}