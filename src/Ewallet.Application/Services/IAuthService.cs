using Ewallet.Application.DTOs.Auth;

namespace Ewallet.Application.Services;

public interface IAuthService
{
    Task<RegisterResult> Register(RegisterRequest request, CancellationToken cancellationToken);
    Task<LoginResult> Login(LoginRequest request, CancellationToken cancellationToken);
}