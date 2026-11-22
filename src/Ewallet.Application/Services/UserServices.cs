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


    public async Task<IReadOnlyList<UserResponse>> GetAllUserAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }

    public async Task<UserResponse?> GetByUserIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

}