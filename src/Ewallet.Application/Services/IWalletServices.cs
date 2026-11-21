using Ewallet.Application.DTOs;

namespace Ewallet.Application.Services;

public interface IWalletServices
{
    Task<WalletResponse?> GetByIdWalletAsync(long id, CancellationToken cancellationToken);
    Task<TransactionsTopUp?> TopUpAsync(long id, TopUpRequest request, CancellationToken cancellationToken);
    
    // Task<PaymentMethodResponse?> GetByIdTopUp(long id, CancellationToken cancellationToken);

    // Task<TransactionResponse?> TransferAsync(TransferRequest request, CancellationToken cancellationToken);
}