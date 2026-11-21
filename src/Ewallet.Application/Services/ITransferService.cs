using Ewallet.Application.DTOs;

namespace Ewallet.Application.Services;

public interface ITransferService
{
    Task<TransactionsTransferResponse?> TransferAsync(TransferRequest request, CancellationToken cancellationToken);
}