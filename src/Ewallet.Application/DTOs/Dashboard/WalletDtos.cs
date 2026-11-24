
namespace Ewallet.Application.DTOs.Dashboard;

public sealed record WalletResponseDashboard(
    long Id,
    string WalletNumber,
    decimal Balance,
    string Status
);

public sealed record WalletMutationResponse(
    long WalletId,
    string Direction,
    decimal Amount,
    decimal BalanceBefore,
    decimal BalanceAfter,
    DateTimeOffset CreatedAtUtc
);

public sealed record UserResponseDashboard(
    long Id,
    string Username,
    string FullName,
    string Email,
    string PhoneNumber,
    string Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    WalletResponseDashboard Wallet,
    IReadOnlyList<WalletMutationResponse> RecentMutations
);
