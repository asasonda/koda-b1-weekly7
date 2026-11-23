using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ewallet.Application.DTOs;
using Ewallet.Application.DTOs.Auth;
using Ewallet.Application.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ewallet.Infrastructure.Identity;

public sealed class JwtTokenService(IOptions<JwtOptions> options) : IJwtTokenService
{
    private readonly JwtOptions _option = options.Value;
    public IssuedAccessToken IssueAccessToken(AuthUserResponse user)
    {
        var now = DateTimeOffset.UtcNow;

        var expiredAtUtc = now.AddMinutes(_option.AccessTokenMinutes);
        var keyBytes = Convert.FromBase64String(_option.SigningKeyBase64);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64)
        };

        foreach (var role in user.Roles.Distinct(StringComparer.Ordinal))
        {
            claims.Add(new Claim(
                "role",
                role
            ));
        }

        var jwt = new JwtSecurityToken(
            issuer: _option.Issuer,
            audience: _option.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiredAtUtc.UtcDateTime,
            signingCredentials: signinCredentials
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        return new IssuedAccessToken(accessToken, expiredAtUtc);
    }
}