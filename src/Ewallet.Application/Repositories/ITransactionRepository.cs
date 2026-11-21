using Ewallet.Application.DTOs;
using EWallet.Domain.Entities;

namespace Ewallet.Application.Repositories;

public interface ITransactionRepository
{
    // Task<Transactions?> AddAsync(Transactions transactions, CancellationToken cancellationToken);
    // Task<IReadOnlyList<Transactions>> GetByUsersIdAsync(int usersId, CancellationToken cancellationToken);

    Task<string?> GetReferenceNo(CancellationToken cancellationToken);
}