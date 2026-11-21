using Ewallet.Application.DTOs.Auth;
using Ewallet.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Ewallet.Infrastructure.Identity;

public sealed class IdentityAuthService(UserManager<AppUser> user) : IAuthService
{
    private readonly UserManager<AppUser> _user = user;
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
            user.Email!,
            user.FullName,
            user.Status,
            user.CreatedAtUtc
        );

        return new LoginResult(true, response);
    }

    public async Task<RegisterResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var email = request.Email.Trim();
        var existingUser = await _user.FindByEmailAsync(email);
        if(existingUser is not null)
        {
            return new RegisterResult(RegisterStatus.DuplicateEmail, null!, ["Email sudah terdaftar"]);
        }

        var user = new AppUser
        {
            UserName = email,
            Email = email,
            FullName = request.FullName,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        IdentityResult identityResult;

        try
        {
            identityResult = await _user.CreateAsync(user, request.Password);
        }
        catch (DbUpdateException e) when (e.InnerException is SqlException sqlError && sqlError.Number is 2601 or 2627)
        {
            return new RegisterResult(RegisterStatus.DuplicateEmail, null!, ["Email sudah terdaftar"]);
        }

        if (!identityResult.Succeeded)
        {
        var error = identityResult.Errors.ToArray();
        var isDuplicate = error.Any(error => error.Code is "DuplicateEmail" or "DuplicateUsername");

        if (isDuplicate)
        {
            return new RegisterResult(RegisterStatus.DuplicateEmail, null!, ["Email sudah terdaftar"]);
        }
        return new RegisterResult(RegisterStatus.ValidationError,null!,error.Select(error => error.Description).ToArray());

        }
        var response = new AuthUserResponse(
            user.Id, 
            user.Email!,
            user.FullName,
            user.Status,
            user.CreatedAtUtc
        );

        return new RegisterResult(RegisterStatus.Created, response, []);
    }
}