using Ewallet.Application.DTOs;
using EWallet.Domain.Entities;
using Ewallet.Application.Repositories;

namespace Ewallet.Application.Services;

public sealed class TransferService : ITransferService
{
    private readonly ITransferRepository _repository;
    private readonly IWalletRepository _repoWallet;
    private readonly ITransactionRepository _repoTransaction;

    public TransferService(ITransferRepository repository, IWalletRepository repoWallet, ITransactionRepository repoTransaction)
    {
        _repository = repository;
        _repoWallet = repoWallet;
        _repoTransaction = repoTransaction;

    }
    public async Task<TransactionsTransferResponse?> TransferAsync(TransferRequest request, CancellationToken cancellationToken)
    {
        if (request.SenderWalletId == request.ReceiverWalletId)
        {
            throw new InvalidOperationException("Tidak dapat transfer ke wallet sendiri");
        }

        if (request.Amount <= 0)
        {
            throw new ArgumentException("Nominal transfer harus lebih dari 0");
        }

        var sender = await _repoWallet.GetByIdWallet(request.SenderWalletId, cancellationToken);
        var receiver = await _repoWallet.GetByIdWallet(request.ReceiverWalletId, cancellationToken);

        if (sender is null || receiver is null)
        {
            return null;
        }

        if (sender.Balance < request.Amount)
        {
            throw new InvalidOperationException("Saldo tidak mencukupi");
        }

        var cekReference = await _repoTransaction.GetReferenceNo(cancellationToken);

        int number = 1;
        if (cekReference is not null)
        {
            number = int.Parse(cekReference[3..]) + 1;
        }

        var referenceNo = $"RF{number:D3}";

        var transaction = new Transactions(
            referenceNo,
            request.SenderWalletId,
            request.ReceiverWalletId,
            "TRANSFER",
            request.Amount,
            request.Description
        );

        var senderBalanceBefore = sender.Balance;
        var receiverBalanceBefore = receiver.Balance;

        sender.ReduceBalance(request.Amount);
        receiver.AddBalance(request.Amount);

        var senderBalanceAfter = sender.Balance;
        var receiverBalanceAfter = receiver.Balance;

        var senderMutation = new WalletMutations(
            transaction,
            sender.Id,
            "OUT",
            request.Amount,
            senderBalanceBefore,
            senderBalanceAfter
        );

        var receiverMutation = new WalletMutations(
            transaction,
            receiver.Id,
            "IN",
            request.Amount,
            receiverBalanceBefore,
            receiverBalanceAfter
        );

        transaction.ConfirmationSuccess();

        var createTransfer = await _repository.TransferAsync(transaction, senderMutation, receiverMutation, cancellationToken);
        if (!createTransfer)
        {
            return null;
        }

        return new TransactionsTransferResponse(
            transaction.Id,
            referenceNo,
            transaction.SenderWalletId,
            transaction.ReceiverWalletId,
            transaction.Type,
            transaction.Amount,
            transaction.Status,
            transaction.CreatedAtUtc,
            transaction.CompletedAtUtc
        );
    }
}