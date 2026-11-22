namespace Ewallet.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public sealed record UpdatedUserRequest(
    [Required(ErrorMessage = "Username wajib diisi")]
    [StringLength(50, ErrorMessage = "Username maksimal 50 karakter")]
    string UserName,

    [Required(ErrorMessage = "Fullname wajib diisi")]
    [StringLength(100, ErrorMessage = "Fullname maksimal 100 karakter")]
    string FullName,

    [Required(ErrorMessage = "Email wajib diisi")]
    [EmailAddress(ErrorMessage = "Format email tidak valid")]
    [StringLength(150)]
    string Email,

    [Required(ErrorMessage = "Nomor telepon wajib diisi")]
    [Phone(ErrorMessage = "Format nomor telepon tidak valid")]
    [StringLength(20)]
    string PhoneNumber,

    string? CurrentPassword,

    [StringLength(20, MinimumLength = 8,
        ErrorMessage = "Password harus 8-20 karakter")]
    string? NewPassword
);

public sealed record WalletResponse(
    long Id, 
    string WalletNumber, 
    decimal Balance, 
    string Status
);

public sealed record UserResponse(
    long Id,
    string Username,
    string FullName,
    string Email,
    string PhoneNumber,
    string Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    WalletResponse Wallet
);

// public sealed record UserResponse(
//     long Id,
//     long AppUserId,
//     string UserName,
//     string FullName,
//     string Email,
//     string PhoneNumber,
//     string Status,
//     DateTime CreatedAtUtc,
//     DateTime? UpdatedAtUtc,
//     WalletResponse Wallet
// );
