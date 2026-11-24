using Ewallet.Application.DTOs;
using EWallet.Domain.Entities;
using Ewallet.Application.Repositories;
using Ewallet.Application.DTOs.Dashboard;

namespace Ewallet.Application.Services;

public sealed class UserServices : IUserServices
{
    private readonly IUserRepository _repository;

    public UserServices(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<AdminDashboardDto> GetAdminDashboardAsync(CancellationToken cancellationToken)
    {
        var users = await _repository.GetAllAsync(cancellationToken);

        var now = DateTime.UtcNow;

        var totalUsers = users.Count;

        var activeUsers = users.Count(user => user.Status == "ACTIVE");
        var suspendedUsers = users.Count(user => user.Status == "SUSPENDED");

        var recentUsers = users
            .OrderByDescending(user => user.CreatedAtUtc)
            .Take(4)
            .Select(user => new RecentUserDto(
                user.Id,
                user.FullName,
                user.Email,
                user.Status.ToString()
            )).ToList();

        return new AdminDashboardDto(
            totalUsers,
            activeUsers,
            suspendedUsers,
            recentUsers
        );
    }

    public async Task<IReadOnlyList<UserResponse>> GetAllUserAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }

    public async Task<UserResponse?> GetByUserIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    public Task<UserResponseDashboard?> GetDashboardAsync(long appUserId, CancellationToken cancellationToken = default)
    {
        return _repository.GetDashboardAsync(appUserId,cancellationToken);
    }

}