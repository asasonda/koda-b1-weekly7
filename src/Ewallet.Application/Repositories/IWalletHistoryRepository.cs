using Ewallet.Application.DTOs;

namespace Ewallet.Application.Repositories;

public interface IWalletHistoryRepository
{
    Task<PagedResult<TransactionHistoryResponse>> GetTransactionsAsync(long walletId, TransactionHistoryCriteria criteria, CancellationToken ct);

    Task<PagedResult<WalletMutationHistoryResponse>> GetMutationsAsync(long walletId, WalletMutationHistoryCriteria criteria, CancellationToken ct);
    Task<bool> WalletExistsAsync(long walletId, CancellationToken ct);
}