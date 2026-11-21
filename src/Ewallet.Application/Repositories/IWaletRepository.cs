using EWallet.Domain.Entities;

namespace Ewallet.Application.Repositories;

public interface IWalletRepository
{
    Task<Wallets?> GetByIdWallet(long walletId, CancellationToken cancellationToken);
    Task<bool> CreateTopUp(Transactions transactions, TopUpRequests topUpRequests, CancellationToken cancellationToken);
    Task<TopUpRequests?> GetByWalletIdTopUp(long id, CancellationToken cancellationToken);
    Task<bool> ReduceBalanceAsync(long id, decimal amount, CancellationToken cancellationToken);
    Task<bool> AddBalanceAsync(long id, decimal amount, CancellationToken cancellationToken);
}