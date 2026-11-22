using Ewallet.Api.Seeding;
using Ewallet.Application.Repositories;
using Ewallet.Application.Services;
using Ewallet.Infrastructure.Identity;
using Ewallet.Infrastructure.Persistence;
using Ewallet.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ewallet.Infrastructure;

public static class DepedencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("EwalletWeekly7") ?? throw new InvalidOperationException("connection belum diconfigurasi");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString)
                .UseSeeding((context, _) =>
                {
                    PaymentMethodSeedData.Seed(context);
                    // UserSeedData.Seed(context);
                })
                .UseAsyncSeeding(async (context, _, ct) =>
                {
                    await PaymentMethodSeedData.SeedAsync(context, ct);
                    // await UserSeedData.SeedAsync(context,ct);
                });
        });

        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<IWalletRepository, EfWalletRepository>();
        services.AddScoped<ITransactionRepository, EfTransactionRepository>();
        services.AddScoped<IPaymentMethod, EfPaymentMethodRepository>();
        services.AddScoped<ITopUpRequest, EfTopUpRequestRespository>();
        services.AddScoped<ITransferRepository, EfTransferRepository>();
        services.AddScoped<IWalletHistoryRepository, EfWalletHistoryRepository>();

        services.AddIdentityCore<AppUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredUniqueChars = 4;

            options.SignIn.RequireConfirmedEmail = false;
        }).AddRoles<IdentityRole<long>>()
        .AddEntityFrameworkStores<AppDbContext>();

        services.AddScoped<IAuthService, IdentityAuthService>();
        return services;
    }
}