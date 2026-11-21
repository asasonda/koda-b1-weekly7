namespace Ewallet.Application.DTOs.Auth;

public sealed record LoginResponse(
    string Message,
    AuthUserResponse User
);