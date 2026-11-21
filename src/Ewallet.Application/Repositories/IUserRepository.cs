using EWallet.Domain.Entities;

namespace Ewallet.Application.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<Users>> GetAllAsync(CancellationToken cancellationToken);
    Task<Users?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<bool> AddUserWithWalletAsync(Users users, Wallets wallets, CancellationToken cancellationToken);
    Task<string?> GetWalletNumberAsync(CancellationToken cancellationToken);
    Task<bool> UpdateUserAsync(Users users, CancellationToken cancellationToken);
}