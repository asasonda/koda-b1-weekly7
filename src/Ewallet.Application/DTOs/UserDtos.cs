using System.ComponentModel.DataAnnotations;

namespace Ewallet.Application.DTOs;

public sealed record CreateUserRequest(
    [Required(ErrorMessage = "Username wajib diisi.")]
    [StringLength(50, ErrorMessage = "Username maksimal 50 karakter.")]
    string UserName,

    [Required(ErrorMessage = "Fullname wajib diisi.")]
    [StringLength(100, ErrorMessage = "Nama maksimal 100 karakter.")]
    string FullName,

    [Required(ErrorMessage = "Email wajib diisi.")]
    [EmailAddress(ErrorMessage = "Format email tidak valid.")]
    [StringLength(150, ErrorMessage = "Email maksimal 100 karakter.")]
    string Email,

    [Required(ErrorMessage = "Nomor telepon wajib diisi.")]
    [StringLength(20, ErrorMessage = "Nomor telepon maksimal 20 karakter.")]
    string Phone,

    [StringLength(500, ErrorMessage = "Password maksimal 500 karakter")]
    string? PasswordHash
);

public sealed record UpdatedUserRequest(
    [Required(ErrorMessage = "Username wajib diisi.")]
    [StringLength(50, ErrorMessage = "Username maksimal 50 karakter.")]
    string UserName,

    [Required(ErrorMessage = "Fullname wajib diisi.")]
    [StringLength(100, ErrorMessage = "Nama maksimal 100 karakter.")]
    string FullName,

    [Required(ErrorMessage = "Email wajib diisi.")]
    [EmailAddress(ErrorMessage = "Format email tidak valid.")]
    [StringLength(150, ErrorMessage = "Email maksimal 100 karakter.")]
    string Email,

    [Required(ErrorMessage = "Nomor telepon wajib diisi.")]
    [StringLength(20, ErrorMessage = "Nomor telepon maksimal 20 karakter.")]
    string Phone,

    [StringLength(500, ErrorMessage = "Password maksimal 500 karakter")]
    string? PasswordHash
);

public sealed record WalletResponse(
    long Id, 
    string WalletNumber, 
    decimal Balance, 
    string Status
);

public sealed record UserResponse(
    long Id, 
    string UserName, 
    string FullName, 
    string Email, 
    string Phone, 
    string Status, 
    DateTime CreatedAtUtc, 
    DateTime? UpdatedAtUtc, 
    WalletResponse Wallet
);