
using Ewallet.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// Sesuaikan namespace AppUser dengan lokasi class AppUser kamu.
using Ewallet.Infrastructure.Identity;
using EWallet.Domain.Entities;

namespace Ewallet.Api.Seeding;

public static class UserSeedData
{
    private static IEnumerable<(string Username, string FullName, string Email, string Phone)> InitialUsers()
    {
        yield return ("Afif", "Afif Sasonda", "afif@gmail.com", "081234567801");
        yield return ("Rizal", "Rizal Kadamong", "rizal@gmail.com", "081234567802");
        yield return ("Zakky", "Zakky", "zakky@gmail.com", "081234567803");
        yield return ("Bando", "Bando Mega", "bando@gmail.com", "081234567804");
        yield return ("Alvin", "Alvin C.", "alvin@gmail.com", "081234567805");
        yield return ("Rifai", "A. Rifai", "rifai@gmail.com", "081234567806");
        yield return ("Chandra", "Chandra A", "chandra@gmail.com", "081234567807");
        yield return ("Dimas", "Dimas B", "dimas@gmail.com", "081234567808");
        yield return ("Andi", "Andianto", "andi@gmail.com", "081234567809");
        yield return ("Budi", "Budiono", "budi@gmail.com", "081234567810");
    }

    public static async Task SeedAsync(
        UserManager<AppUser> userManager,
        AppDbContext db,
        CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(ct);

        try
        {
            int number = 1;

            foreach (var item in InitialUsers())
            {
                var email = item.Email;

                var appUser = await userManager.FindByEmailAsync(email);

                if (appUser is null)
                {
                    appUser = new AppUser
                    {
                        UserName = item.Username,
                        Email = email,
                        PhoneNumber = item.Phone,
                        FullName = item.FullName
                    };

                    var result = await userManager.CreateAsync(
                        appUser,
                        "Seed@12345"
                    );

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(
                            ", ",
                            result.Errors.Select(e => e.Description)
                        );

                        throw new InvalidOperationException(
                            $"Gagal membuat user {email}: {errors}"
                        );
                    }
                }

                bool walletExists = await db.Wallets
                    .AnyAsync(w => w.AppUserId == appUser.Id, ct);

                if (!walletExists)
                {
                    var walletNumber = $"WAL{number:D3}";

                    var wallet = new Wallets(
                        appUser.Id,
                        walletNumber,
                        0
                    );

                    db.Wallets.Add(wallet);
                }

                number++;
            }

            await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}
