using Ewallet.Application.DTOs;
using EWallet.Domain.Entities;

namespace Ewallet.Application.Repositories;

public interface ITopUpRequest
{
    Task<TopUpRequests?> GetByWalledIdTopUp(long walletId, CancellationToken cancellationToken);
    Task<TopUpRequests?> GetByIdTopUp(long walletId, CancellationToken cancellationToken);
    Task<bool> ConfirmAsync(long id, CancellationToken cancellationToken);
}