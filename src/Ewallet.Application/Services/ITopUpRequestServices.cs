using Ewallet.Application.DTOs;

namespace Ewallet.Application.Services;

public interface ITopUpRequestServices
{
    Task<TopUpResponse?> GetByWalletIdTopUpAsync(long walletId, CancellationToken cancellationToken); 
    Task<TopUpResponse?> ConfirmTopUpAsync(long id, CancellationToken cancellationToken);
}