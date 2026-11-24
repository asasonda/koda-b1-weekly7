using Ewallet.Application.DTOs;
using Ewallet.Application.DTOs.Dashboard;
using EWallet.Domain.Entities;

namespace Ewallet.Application.Services;

public interface IUserServices
{
    Task<IReadOnlyList<UserResponse>> GetAllUserAsync(CancellationToken cancellationToken);
    Task<UserResponse?> GetByUserIdAsync(long id, CancellationToken cancellationToken);


    Task<UserResponseDashboard?> GetDashboardAsync(long appUserId,CancellationToken cancellationToken);
    Task<AdminDashboardDto> GetAdminDashboardAsync(CancellationToken cancellationToken);


    // Task<UserResponse?> UpdateUserAsync(long id, UpdatedUserRequest request, CancellationToken cancellationToken);
    // Task<UserResponse?> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
    // Task<IReadOnlyList<TransactionResponse>> GetTransactionsByUsersIdAsync(int usersId, CancellationToken cancellationToken);
}