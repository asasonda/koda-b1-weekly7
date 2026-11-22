using Ewallet.Application.DTOs.Auth;
using Ewallet.Application.Repositories;
using Ewallet.Application.Services;
using Ewallet.Infrastructure.Persistence;
using EWallet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ewallet.Infrastructure.Identity;

public sealed class IdentityAuthService(UserManager<AppUser> user, AppDbContext db, IUserRepository repository) : IAuthService
{
    private readonly UserManager<AppUser> _user = user;
    private readonly AppDbContext _db = db;
    private readonly IUserRepository _repository = repository;
    public async Task<LoginResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var email = request.Email.Trim();
        var user = await _user.FindByEmailAsync(email);

        if(user is null)
        {
            return new LoginResult(false, null);
        }

        var passwordValid = await _user.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
        {
            return new LoginResult(false, null);
        }

        var response = new AuthUserResponse(
            user.Id, 
            user.UserName!,
            user.Email!,
            user.Status,
            user.CreatedAtUtc,
            user.UpdatedAtUtc
        );

        return new LoginResult(true, response);
    }

    public async Task<RegisterResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var email = request.Email.Trim();
        var existingUser = await _user.FindByEmailAsync(email);
        if(existingUser is not null) return new RegisterResult(RegisterStatus.DuplicateEmail, null!, ["Email sudah terdaftar"]);

        var username = request.UserName.Trim();
        var existingUsername = await _user.FindByNameAsync(username);
        if (existingUsername is not null) return new RegisterResult(RegisterStatus.DuplicateUserName,null!,["Username sudah terdaftar"]);

        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var appUser = new AppUser
            {
                UserName = request.UserName.Trim(),
                Email = email,
                PhoneNumber = request.PhoneNumber.Trim(),
                FullName = request.FullName.Trim()
            };

            var identityResult = await _user.CreateAsync(appUser, request.Password);

            if (!identityResult.Succeeded)
            {
                await transaction.RollbackAsync(CancellationToken.None);
                var error = identityResult.Errors.ToArray();
                var isDuplicate = error.Any(error => error.Code == "DuplicateEmail");
                var isDuplicateUsername = error.Any(e => e.Code == "DuplicateUserName");

                if (isDuplicate) return new RegisterResult(RegisterStatus.DuplicateEmail, null!, ["Email sudah terdaftar"]);
                if (isDuplicateUsername) return new RegisterResult(RegisterStatus.DuplicateUserName, null!,["Username sudah terdaftar"]);
                
                return new RegisterResult(RegisterStatus.ValidationError,null!,error.Select(error => error.Description).ToArray());
            }

            var cekWalletNumber = await _repository.GetWalletNumberAsync(cancellationToken);

            int number = 1;
            if(cekWalletNumber is not null)
            {
                number = int.Parse(cekWalletNumber[3..]) + 1;
            }
            var walletNumber = $"WAL{number:D3}";

            var wallet = new Wallets(
                appUser.Id,
                walletNumber,
                0
            );

            var created = await _repository.AddUserWithWalletAsync(wallet, cancellationToken);
            if (!created)
            {
                await transaction.RollbackAsync(CancellationToken.None);
                return new RegisterResult(RegisterStatus.ValidationError,null!,["Created failed!"]);
            }

            await transaction.CommitAsync(cancellationToken);

            var response = new AuthUserResponse(
                appUser.Id,
                appUser.UserName!,
                appUser.Email!,
                appUser.Status,
                appUser.CreatedAtUtc,
                appUser.UpdatedAtUtc
            );

            return new RegisterResult(
                RegisterStatus.Created,
                response,
                []
            );
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}