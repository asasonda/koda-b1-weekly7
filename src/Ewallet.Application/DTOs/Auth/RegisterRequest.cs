using System.ComponentModel.DataAnnotations;

namespace Ewallet.Application.DTOs.Auth;

public sealed class RegisterRequest
{
    [Required(ErrorMessage ="Email wajib diisi")]
    [StringLength(150, ErrorMessage = "panjang karakter max 150")]
    [EmailAddress(ErrorMessage ="Harus sesuai format email")]
    public string Email {get; set;} =string.Empty;

    [Required(ErrorMessage = "Password wajib diisi")]
    [StringLength(20, MinimumLength = 8)]
    public string Password {get; set;} = string.Empty;

    [Required(ErrorMessage ="Fullname wajib diisi")]
    [StringLength(100, ErrorMessage ="panjang karakter max 100")]
    public string FullName {get; set;} = string.Empty;

}