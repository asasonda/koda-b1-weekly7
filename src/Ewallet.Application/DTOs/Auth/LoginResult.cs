namespace Ewallet.Application.DTOs.Auth;

public sealed record LoginResult(
    bool Succeeded,
    AuthUserResponse? User
);