
using EWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ewallet.Api.Seeding;

public static class UserSeedData
{
    private static IEnumerable<Users> InitialUsers()
    {
        yield return new Users("Afif", "Afif sasonda", "afif@gmail.com", "081234567801", null);
        yield return new Users("Rizal", "rizal kadamong", "rizal@gmail.com", "081234567802", null);
        yield return new Users("Zakky", "zakyyyyyy", "zakky@gmail.com", "081234567803", null);
        yield return new Users("Bando", "Bando mega", "bando@gmail.com", "081234567804", null);
        yield return new Users("Alvin", "Alvin C.", "alvin@gmail.com", "081234567805", null);
        yield return new Users("Rifai", "A. Rifai", "rifai@gmail.com", "081234567806", null);
        yield return new Users("Chandra", "Chandar A", "chandra@gmail.com", "081234567807", null);
        yield return new Users("Dimas", "Dimas B", "dimas@gmail.com", "081234567808", null);
        yield return new Users("Andi", "Andianto", "andi@gmail.com", "081234567809", null);
        yield return new Users("Budi", "Budiono", "budi@gmail.com", "081234567810", null);
    }

    public static void Seed(DbContext context)
    {
        int number = 1;

        foreach (var user in InitialUsers())
        {
            bool exists = context.Set<Users>()
                .Any(u => u.Email == user.Email);

            if (exists)
                continue;

            var walletNumber = $"WAL{number:D3}";

            var wallet = new Wallets(
                user,
                walletNumber,
                0
            );

            context.Set<Users>().Add(user);
            context.Set<Wallets>().Add(wallet);

            number++;
        }

        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken ct)
    {
        int number = 1;

        foreach (var user in InitialUsers())
        {
            bool exists = await context.Set<Users>()
                .AnyAsync(u => u.Email == user.Email, ct);

            if (exists)
                continue;

            var walletNumber = $"WAL{number:D3}";

            var wallet = new Wallets(
                user,
                walletNumber,
                0
            );
            context.Set<Users>().Add(user);
            context.Set<Wallets>().Add(wallet);
            number++;
        }
        await context.SaveChangesAsync(ct);
    }
}
