using Ewallet.Application.DTOs;
using EWallet.Domain.Entities;
using Ewallet.Application.Repositories;

namespace Ewallet.Application.Services;

public sealed class UserServices : IUserServices
{
    private readonly IUserRepository _repository;

    public UserServices(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserResponse?> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var users = new Users(
            request.UserName,
            request.FullName,
            request.Email,
            request.Phone,
            null
        );

        var cekWalletNumber = await _repository.GetWalletNumberAsync(cancellationToken);

        int number = 1;

        if(cekWalletNumber is not null)
        {
            number = int.Parse(cekWalletNumber[3..]) + 1;
        }

        var walletNumber = $"WAL{number:D3}";

        var wallet = new Wallets(
            users,
            walletNumber,
            0
        );

        var created = await _repository.AddUserWithWalletAsync(users, wallet, cancellationToken);

        if (!created)
        {
            return null;
        }

        return new UserResponse(
            users.Id,
            users.UserName,
            users.FullName,
            users.Email,
            users.Phone,
            users.Status,
            users.CreatedAtUtc,
            users.UpdatedAtUtc,
            new WalletResponse(
                users.Wallets.Id,
                users.Wallets.WalletNumber,
                users.Wallets.Balance,
                users.Wallets.Status
            )
        );
    }

    public async Task<IReadOnlyList<UserResponse>> GetAllUserAsync(CancellationToken cancellationToken)
    {
        var users = await _repository.GetAllAsync(cancellationToken);

        return users.Select(u => new UserResponse
        (
            u.Id,
            u.UserName,
            u.FullName,
            u.Email,
            u.Phone,
            u.Status,
            u.CreatedAtUtc,
            u.UpdatedAtUtc,
            new WalletResponse(u.Wallets.Id, u.Wallets.WalletNumber, u.Wallets.Balance, u.Wallets.Status)
        )).ToArray();
    }

    public async Task<UserResponse?> GetByUserIdAsync(long id, CancellationToken cancellationToken)
    {
        var users = await _repository.GetByIdAsync(id, cancellationToken);
        if(users is null)
        {
            return null;
        }

        return new UserResponse(
            users.Id,
            users.UserName,
            users.FullName,
            users.Email,
            users.Phone,
            users.Status,
            users.CreatedAtUtc,
            users.UpdatedAtUtc,
            new WalletResponse(
                users.Wallets.Id,
                users.Wallets.WalletNumber,
                users.Wallets.Balance,
                users.Wallets.Status
            )
        );
    }

    public async Task<UserResponse?> UpdateUserAsync(long id, UpdatedUserRequest request, CancellationToken cancellationToken)
    {
        var users = await _repository.GetByIdAsync(id, cancellationToken);

        if(users is null)
        {
            return null;
        }

        users.UpdateDetail(
            request.UserName,
            request.FullName,
            request.Email,
            request.Phone,
            request.PasswordHash
        );

        var update = await _repository.UpdateUserAsync(users, cancellationToken);
        if (!update)
        {
            return null;
        }

        return new UserResponse(
            users.Id,
            users.UserName,
            users.FullName,
            users.Email,
            users.Phone,
            users.Status,
            users.CreatedAtUtc,
            users.UpdatedAtUtc,
            new WalletResponse(
                users.Wallets.Id,
                users.Wallets.WalletNumber,
                users.Wallets.Balance,
                users.Wallets.Status
            )
        );
    }

}