using System.ComponentModel.DataAnnotations;

namespace Ewallet.Application.DTOs.Auth;

public sealed class LoginRequest
{
    [EmailAddress(ErrorMessage = "Harus sesuai format email")]
    [Required(ErrorMessage = "Email wajib diisi")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password wajib diisi")]
    public string Password { get; init;} = string.Empty;
}