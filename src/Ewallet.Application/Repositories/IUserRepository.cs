using Ewallet.Application.DTOs;
using Ewallet.Application.DTOs.Dashboard;
using EWallet.Domain.Entities;

namespace Ewallet.Application.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<UserResponse?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<bool> AddUserWithWalletAsync(Wallets wallets, CancellationToken cancellationToken);
    Task<string?> GetWalletNumberAsync(CancellationToken cancellationToken);

    Task<UserResponseDashboard?> GetDashboardAsync(long appUserId,CancellationToken cancellationToken);
}