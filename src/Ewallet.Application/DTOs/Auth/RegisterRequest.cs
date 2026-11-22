using System.ComponentModel.DataAnnotations;

namespace Ewallet.Application.DTOs.Auth;

public sealed class RegisterRequest
{
    [Required(ErrorMessage = "Username wajib diisi")]
    [StringLength(50, ErrorMessage = "Username maksimal 50 karakter")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Fullname wajib diisi")]
    [StringLength(100, ErrorMessage = "fullname maksimal 100 karakter")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage ="Email wajib diisi")]
    [StringLength(150, ErrorMessage = "panjang karakter max 150")]
    [EmailAddress(ErrorMessage ="Harus sesuai format email")]
    public string Email {get; set;} =string.Empty;

    [Required(ErrorMessage = "Password wajib diisi")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "Password harus 8-20 karakter")]
    public string Password {get; set;} = string.Empty;

    [Required(ErrorMessage = "Nomor telepon wajib diisi")]
    [Phone(ErrorMessage = "Format nomor telepon tidak valid")]
    [StringLength(20, ErrorMessage = "Nomor telepon maksimal 20 karakter")]
    public string PhoneNumber { get; set; } = string.Empty;


}