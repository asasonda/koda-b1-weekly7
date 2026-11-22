namespace Ewallet.Application.DTOs.Auth;

public enum RegisterStatus
{
    Created, DuplicateEmail, DuplicateUserName, ValidationError
}

public sealed record RegisterResult
(
    RegisterStatus Status,
    AuthUserResponse User,
    string[] Error
);