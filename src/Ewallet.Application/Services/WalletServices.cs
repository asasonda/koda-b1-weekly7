using Ewallet.Application.DTOs;
using EWallet.Domain.Entities;
using Ewallet.Application.Repositories;

namespace Ewallet.Application.Services;

public sealed class WalletServices : IWalletServices
{
    private readonly IWalletRepository _repository;
    private readonly IPaymentMethod _paymentMethod;
    private readonly ITransactionRepository _transactionRepository;

    public WalletServices(IWalletRepository repository, IPaymentMethod paymentMethod, ITransactionRepository transactionRepository)
    {
        _repository = repository;
        _paymentMethod = paymentMethod;
        _transactionRepository = transactionRepository;
    }

    public async Task<WalletResponse?> GetByIdWalletAsync(long id, CancellationToken cancellationToken)
    {
        var wallets = await _repository.GetByIdWallet(id, cancellationToken);

        if(wallets is null)
        {
            return null;
        }
        return new WalletResponse(
            wallets.Id,
            wallets.WalletNumber,
            wallets.Balance,
            wallets.Status
        );
    }

    public async Task<TransactionsTopUp?> TopUpAsync(long walletId, TopUpRequest request, CancellationToken cancellationToken)
    {
        if(request.Amount <= 0)
        {
            throw new ArgumentException("Nominal harus lebih dari 0");
        }

        var cekReference = await _transactionRepository.GetReferenceNo(cancellationToken);

        int number = 1;
        if(cekReference is not null)
        {
            number = int.Parse(cekReference[3..]) + 1;
        }
        var referenceNo = $"RF{number:D3}";

        var transaction = new Transactions(
            referenceNo,
            walletId,
            "TOPUP",
            request.Amount,
            "TopUp Saldo"
        );

        var paymentMethod = await _paymentMethod.GetByIdAsync(request.PaymentMethodId, cancellationToken);

        if (paymentMethod is null)
        {
            throw new ArgumentException("Payment method tidak ditemukan");
        }

        if (!paymentMethod.IsActive)
        {
            throw new ArgumentException("Payment method tidak aktif");
        }

        var topUp = new TopUpRequests(
            transaction,
            walletId,
            request.PaymentMethodId,
            request.Amount,
            null
        );

        var created = await _repository.CreateTopUp(transaction, topUp, cancellationToken);

        if (!created)
        {
            return null;
        }

        return new TransactionsTopUp(
            transaction.Id,
            transaction.ReferenceNo,
            transaction.ReceiverWalletId,
            transaction.Type,
            transaction.Amount,
            transaction.Status,
            new TopUpSummaryResponse(
                topUp.Id,
                new PaymentMethodResponse(
                    paymentMethod.Id,
                    paymentMethod.Code,
                    paymentMethod.Name
                ),
                topUp.ExpiresAtUtc
            )

        );
    }
}