namespace Ewallet.Application.DTOs.Auth;

public sealed record AuthUserResponse(
    long Id,
    string Email,
    string FullName,
    string Status,
    DateTimeOffset CreatedAtUtc
);