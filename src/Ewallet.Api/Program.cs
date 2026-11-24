using System.IdentityModel.Tokens.Jwt;
using System.Threading.RateLimiting;
using Ewallet.Api.Middlewares;
using Ewallet.Api.Seeding;
using Ewallet.Application.Services;
using Ewallet.Infrastructure;
using Ewallet.Infrastructure.Identity;
using Ewallet.Infrastructure.Persistence;
using Ewallet.Infrastructure.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Routing.Tree;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails(
    option =>
    {
        option.CustomizeProblemDetails = context =>
        {
            context.ProblemDetails.Instance ??= context.HttpContext.Request.Path.Value;

            context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
        };
    }
);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var jwtOption = builder.Configuration
    .GetSection(JwtOptions
    .SectionName).Get<JwtOptions>() ?? throw new InvalidOperationException("Konfigurasi JWT belum tersedia");

if (string.IsNullOrWhiteSpace(jwtOption.Issuer))
{
    throw new InvalidOperationException("Jwt: Issuer wajib diisi");
}

if (string.IsNullOrWhiteSpace(jwtOption.Audience))
{
    throw new InvalidOperationException("Jwt: Audience wajib diisi");
}

if (jwtOption.AccessTokenMinutes is < 1 or > 60)
{
    throw new InvalidOperationException("Jwt: ACCESS Token Minutes antara 1 - 60");
}
byte[] signingKeyBytes;
try
{
    signingKeyBytes = Convert.FromBase64String(jwtOption.SigningKeyBase64);
}
catch (FormatException e)
{
    throw new InvalidOperationException("Jwt: SigingKeyBase64 tidak menggunakan Base64 yg valid", e);
}

if (signingKeyBytes.Length < 32)
{
    throw new InvalidOperationException("JWT signing key minimal harus 32 byte");
}


builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IWalletServices, WalletServices>();
builder.Services.AddScoped<ITopUpRequestServices, TopUpRequestService>();
builder.Services.AddScoped<IPaymentMethodServices, PaymentMethodServices>();
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddScoped<IWalletHistoryService, WalletHistoryService>();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(option =>
{
    option.AddPolicy("ReactClient", policy =>
    {
        policy.WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddRateLimiter(option =>
{
    option.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    option.AddPolicy("auth-login", context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "Nihil";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: $"login:{ip}",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(3),
                QueueLimit = 0,
                AutoReplenishment = true
            }
        );
    });

    option.AddPolicy("auth-register", context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "Nihil";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: $"register:{ip}",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(10),
                QueueLimit = 0,
                AutoReplenishment = true
            }
        );
    });

    option.AddPolicy("auth-refresh", context =>
    {
       var ip = context.Connection.RemoteIpAddress?.ToString() ?? "Nihil";
       return RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: $"refresh:{ip}",
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(2),
            QueueLimit = 0,
            AutoReplenishment = true
        }
       ); 
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.MapInboundClaims = false;
        option.IncludeErrorDetails = false;
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOption.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtOption.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),
            RequireSignedTokens = true,
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero,
            NameClaimType = JwtRegisteredClaimNames.Sub,
            RoleClaimType = "role"
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var db = services.GetRequiredService<AppDbContext>();

    await UserSeedData.SeedAsync(
        userManager,
        db,
        CancellationToken.None
    );
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    var bootstrapAdminEmail = app.Configuration["RoleBootstrap:AdminEmail"];

    await RoleSeedData.SeedAsync(
        roleManager,
        userManager,
        bootstrapAdminEmail);
        
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("ReactClient");

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();