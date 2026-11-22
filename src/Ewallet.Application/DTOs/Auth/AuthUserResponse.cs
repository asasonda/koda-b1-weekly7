namespace Ewallet.Application.DTOs.Auth;

public sealed record AuthUserResponse(
    long Id,
    string UserName,
    string Email,
    string Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc
);