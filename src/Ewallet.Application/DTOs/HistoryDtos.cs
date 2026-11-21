namespace Ewallet.Application.DTOs;

public sealed record TransactionHistoryResponse(
    long Id,
    string ReferenceNo,
    long? SenderWalletId,
    long ReceiverWalletId,
    string Type,
    decimal Amount,
    string Status,
    string? Description,
    DateTime CreatedAtUtc,
    DateTime? CompletedAtUtc
);

public sealed record TransactionHistoryRequest(
    string? ReferenceNo,
    string? Type,
    string? Status,
    DateTime? FromDate,
    DateTime? ToDate,
    string? Sort = "date_desc",
    int Page = 1,
    int PageSize = 10
);


public sealed record WalletMutationHistoryResponse(
    long Id,
    string ReferenceNo,
    long WalletId,
    string Direction,
    decimal Amount,
    decimal BalanceBefore,
    decimal BalanceAfter,
    DateTime CreatedAtUtc
);

public sealed record WalletMutationHistoryRequest(
    string? ReferenceNo,
    string? Direction,
    DateTime? FromDate,
    DateTime? ToDate,
    string? Sort = "date_desc",
    int Page = 1,
    int PageSize = 10
);

public sealed record PagedResult<T>(
    int Page,
    int PageSize,
    long TotalCount,
    long TotalPages,
    IReadOnlyList<T> Items
);

public sealed record TransactionHistoryCriteria(
    string? ReferenceNo,
    string? Type,
    string? Status,
    DateTime? FromDate,
    DateTime? ToDate,
    string Sort,
    int Page,
    int PageSize
);

public sealed record WalletMutationHistoryCriteria(
    string? ReferenceNo,
    string? Direction,
    DateTime? FromDate,
    DateTime? ToDate,
    string Sort,
    int Page,
    int PageSize
);