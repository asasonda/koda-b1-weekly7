using Ewallet.Application.DTOs;

namespace Ewallet.Application.Services;

public interface IWalletHistoryService
{
    Task<PagedResult<TransactionHistoryResponse>> GetTransactionsAsync(long walletId, TransactionHistoryRequest request, CancellationToken ct);
    Task<PagedResult<WalletMutationHistoryResponse>> GetMutationsAsync(long walletId, WalletMutationHistoryRequest request, CancellationToken ct);
}