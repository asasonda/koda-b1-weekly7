using Ewallet.Application.DTOs;
using EWallet.Domain.Entities;
using Ewallet.Application.Repositories;

namespace Ewallet.Application.Services;

public sealed class TopUpRequestService : ITopUpRequestServices
{
    private readonly ITopUpRequest _repository;

    public TopUpRequestService(ITopUpRequest repository)
    {
        _repository = repository;
    }

    public async Task<TopUpResponse?> ConfirmTopUpAsync(long id, CancellationToken cancellationToken)
    {
        var confirm = await _repository.ConfirmAsync(id, cancellationToken);

        if (!confirm)
        {
            throw new ArgumentException("Gagal dikonfirmasi");
        }

        var topUp = await _repository.GetByIdTopUp(id, cancellationToken);

        return new TopUpResponse(
            topUp.Id,
            topUp.TransactionId,
            topUp.WalletId,
            new PaymentMethodResponse(
                topUp.PaymentMethods.Id,
                topUp.PaymentMethods.Code,
                topUp.PaymentMethods.Name
            ),
            topUp.Amount,
            topUp.Status,
            topUp.CreatedAtUtc,
            topUp.ExpiresAtUtc
        );
    }

    public async Task<TopUpResponse?> GetByWalletIdTopUpAsync(long walletId, CancellationToken cancellationToken)
    {
        if(walletId <= 0)
        {
            throw new ArgumentException("Id tidak valid");
        }

        var topUp = await _repository.GetByWalledIdTopUp(walletId,cancellationToken);
        if(topUp is null)
        {
            return null;
        }

        return new TopUpResponse(
            topUp.Id,
            topUp.TransactionId,
            topUp.WalletId,
            new PaymentMethodResponse(
                topUp.PaymentMethods.Id,
                topUp.PaymentMethods.Code,
                topUp.PaymentMethods.Name
            ),
            topUp.Amount,
            topUp.Status,
            topUp.CreatedAtUtc,
            topUp.ExpiresAtUtc
        );
    }
}