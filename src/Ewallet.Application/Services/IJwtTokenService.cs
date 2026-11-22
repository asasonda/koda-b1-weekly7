using Ewallet.Application.DTOs;
using Ewallet.Application.DTOs.Auth;

namespace Ewallet.Application.Services;

public interface IJwtTokenService
{
    IssuedAccessToken IssueAccessToken(AuthUserResponse user);
}