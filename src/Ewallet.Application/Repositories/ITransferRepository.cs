using EWallet.Domain.Entities;

namespace Ewallet.Application.Repositories;

public interface ITransferRepository
{
    Task<bool> TransferAsync(Transactions transactions, WalletMutations senderMutation, WalletMutations receiverMutation, CancellationToken cancellationToken); 
}