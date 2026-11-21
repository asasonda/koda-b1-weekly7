using EWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ewallet.Api.Seeding;

public sealed class PaymentMethodSeedData
{
    private static IEnumerable<PaymentMethods> InitialUsers()
    {
        yield return new PaymentMethods("BANK_TRANSFER", "Bank BCA", null, true);
        yield return new PaymentMethods("QRIS", "Qris", null, true);
        yield return new PaymentMethods("GoPay", "Gopay", null, false);
    }

    public static void Seed(DbContext context)
    {
        foreach (var method in InitialUsers())
        {
            bool exists = context.Set<PaymentMethods>().Any(pm => pm.Code == method.Code);

            if (!exists)
            {
                context.Set<PaymentMethods>().Add(method);
            }
        }
        context.SaveChanges();
    }
    public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken)
    {
        foreach (var method in InitialUsers())
        {
            bool exists = await context.Set<PaymentMethods>().AnyAsync(pm => pm.Code == method.Code, cancellationToken);

            if (!exists)
            {
                context.Set<PaymentMethods>().Add(method);
            }
        }
        await context.SaveChangesAsync(cancellationToken);
    }
}