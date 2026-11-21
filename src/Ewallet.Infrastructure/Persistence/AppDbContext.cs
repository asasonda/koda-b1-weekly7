using Ewallet.Infrastructure.Identity;
using EWallet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Ewallet.Infrastructure.Persistence;

public sealed class AppDbContext : IdentityDbContext<AppUser, IdentityRole<long>, long>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Users> Users => Set<Users>();
    public DbSet<Wallets> Wallets => Set<Wallets>();
    public DbSet<Transactions> Transactions => Set<Transactions>();
    public DbSet<WalletMutations> WalletMutations => Set<WalletMutations>();
    public DbSet<PaymentMethods> PaymentMethods => Set<PaymentMethods>();
    public DbSet<TopUpRequests> TopUpRequests => Set<TopUpRequests>();
    public DbSet<AuditLogs> AuditLogs => Set<AuditLogs>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.Property(user => user.FullName).IsRequired().HasMaxLength(100);
            entity.Property(user => user.Status).IsRequired().HasMaxLength(20);
            entity.Property(user => user.CreatedAtUtc).IsRequired().HasColumnType("datetimeoffset");
            entity.Property(user => user.UpdatedAtUtc).HasColumnType("datetimeoffset");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.ToTable("Users", "dbo", table =>
            {
               table.HasCheckConstraint("CK_Users_Status", "[Status] in ('ACTIVE', 'SUSPENDED')") ;
            });

            entity.HasKey(u => u.Id);

            entity.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);
            
            entity.Property(u => u.Phone)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(u => u.PasswordHash)
                .HasMaxLength(500);
            
            entity.Property(u => u.Status)
                .IsRequired()
                .HasMaxLength(20);
            
            entity.Property(u => u.CreatedAtUtc)
                .IsRequired()
                .HasColumnType("datetime");
            
            entity.Property(u => u.UpdatedAtUtc)
                .HasColumnType("datetime");

            entity.HasIndex(u => u.UserName).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Phone).IsUnique();

        });

        modelBuilder.Entity<Wallets>(entity =>
        {
            entity.ToTable("Wallets", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Wallets_Balance", "[Balance] >= 0");
                table.HasCheckConstraint("CK_Wallets_Status", "[Status] in ('ACTIVE', 'FROZEN','CLOSED')");
            });

            entity.HasKey(w => w.Id);

            entity.Property(w => w.UsersId)
                .IsRequired();

            entity.Property(w => w.WalletNumber)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(w => w.Balance)
                .IsRequired()
                .HasPrecision(18,2);

            entity.Property(w => w.Status)
                .IsRequired()
                .HasMaxLength(20);
            
            entity.Property(w => w.RowVersion)
                .IsRequired()
                .IsRowVersion();

            entity.Property(w => w.CreatedAtUtc)
                .IsRequired()
                .HasColumnType("datetime");
            
            entity.Property(w => w.UpdatedAtUtc)
                .HasColumnType("datetime");

            entity.HasOne(w => w.Users)
                .WithOne(u => u.Wallets)
                .HasForeignKey<Wallets>(f => f.UsersId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(w => w.UsersId).IsUnique();
            entity.HasIndex(w => w.WalletNumber).IsUnique();
        });

        modelBuilder.Entity<Transactions>(entity =>
        {
            entity.ToTable("Transactions" , "dbo", table =>
            {
                table.HasCheckConstraint("CK_Transaction_Amount", "[Amount] > 0");
                table.HasCheckConstraint("CK_Transaction_Status", "[Status] in ('PENDING', 'SUCCESS', 'FAILED')");
                table.HasCheckConstraint("CK_Transaction_Type",
                    """
                        ([Type] = 'TOPUP' AND [SenderWalletId] is null) or
                        ([Type] = 'TRANSFER' AND [SenderWalletId] is not null AND [SenderWalletId] <> [ReceiverWalletId])
                    """);
            });

            entity.HasKey(t => t.Id);

            entity.Property(t => t.ReferenceNo)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(t => t.SenderWalletId);

            entity.Property(t => t.ReceiverWalletId)
                .IsRequired();

            entity.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(20);
            
            entity.Property(t => t.Amount)
                .IsRequired()
                .HasPrecision(18,2);
            
            entity.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(t => t.Description)
                .HasMaxLength(255);
            
            entity.Property(t => t.CreatedAtUtc)
                .IsRequired()
                .HasColumnType("datetime");
            
            entity.Property(t => t.CompletedAtUtc)
                .HasColumnType("datetime");
            

            entity.HasOne(t => t.WalletsSender)
                .WithMany(w => w.TransactionsSender)
                .HasForeignKey(f => f.SenderWalletId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.WalletsReceiver)
                .WithMany(w => w.TransactionsReceiver)
                .HasForeignKey(f => f.ReceiverWalletId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(t => t.ReferenceNo).IsUnique();
            entity.HasIndex(i => new
            {
               i.SenderWalletId,
               i.CreatedAtUtc 
            }).IsDescending(false, true);

            entity.HasIndex(i => new
            {
               i.ReceiverWalletId,
               i.CreatedAtUtc 
            }).IsDescending(false, true);
        });

        modelBuilder.Entity<WalletMutations>(entity =>
        {
            entity.ToTable("WalletMutations", "dbo", table =>
            {
                table.HasCheckConstraint("CK_WalletMutation_Amount", "[Amount] > 0");
                table.HasCheckConstraint("CK_WalletMutation_Direction", "[Direction] in ('IN', 'OUT')");
            });

            entity.HasKey(wm => wm.Id);

            entity.Property(wm => wm.TransactionsId);
            
            entity.Property(wm => wm.WalletId);

            entity.Property(wm => wm.Direction)
                .IsRequired()
                .HasMaxLength(3);
            
            entity.Property(wm => wm.Amount)
                .IsRequired()
                .HasPrecision(18,2);

            entity.Property(wm => wm.BalanceBefore)
                .IsRequired()
                .HasPrecision(18,2);
            
            entity.Property(wm => wm.BalanceAfter)
                .IsRequired()
                .HasPrecision(18,2);
            
            entity.Property(wm => wm.CreatedAtUtc)
                .IsRequired()
                .HasColumnType("datetime");
            
            entity.HasOne(wm => wm.Transactions)
                .WithMany(t => t.WalletMutations)
                .HasForeignKey(f => f.TransactionsId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(wm => wm.Wallets)
                .WithMany(w => w.WalletMutations)
                .HasForeignKey(f => f.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(i => new
            {
                i.WalletId,
                i.CreatedAtUtc,
                i.Id
            }).IsDescending(false, true, true);
        });

        modelBuilder.Entity<PaymentMethods>(entity =>
        {
            entity.ToTable("PaymentMethods" , "dbo", table =>
            {
               table.HasCheckConstraint("CK_PaymentMethod_Code", "[Code] in ('BANK_TRANSFER', 'QRIS', 'GoPay')"); 
            });

            entity.HasKey(pm => pm.Id);

            entity.Property(pm => pm.Code)
                .IsRequired()
                .HasMaxLength(30);
            
            entity.Property(pm => pm.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(pm => pm.ProviderName)
                .HasMaxLength(100);
            
            entity.Property(pm => pm.IsActive)
                .IsRequired();

            entity.Property(pm => pm.CreatedAtUtc)
                .HasColumnType("datetime");

            entity.HasIndex(i => i.Code).IsUnique();
        });

        modelBuilder.Entity<TopUpRequests>(entity =>
        {
            entity.ToTable("TopUpRequests", "dbo", table =>
            {
                table.HasCheckConstraint("CK_TopUpRequest_Amount", "[Amount] > 0");
                table.HasCheckConstraint("CK_TopUpRequest_Status", "[Status] in ('PENDING', 'PAID', 'FAILED', 'EXPIRED')");
            });

            entity.HasKey(tp => tp.Id);

            entity.Property(tp => tp.TransactionId)
                .IsRequired();
            
            entity.Property(tp => tp.WalletId)
                .IsRequired();
            
            entity.Property(tp => tp.PaymentMethodId)
                .IsRequired();
            
            entity.Property(tp => tp.Amount)
                .IsRequired()
                .HasPrecision(18,2);
            
            entity.Property(tp => tp.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(tp => tp.ProviderReference)
                .HasMaxLength(100);
            
            entity.Property(tp => tp.CreatedAtUtc)
                .IsRequired()
                .HasColumnType("datetime");

            entity.Property(tp => tp.ExpiresAtUtc)
                .HasColumnType("datetime");
            
            entity.Property(tp => tp.PaidAtUtc)
                .HasColumnType("datetime");

            entity.HasOne(tp => tp.Transactions)
                .WithOne(t => t.TopUpRequests)
                .HasForeignKey<TopUpRequests>(f => f.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(tp => tp.Wallets)
                .WithMany(w => w.TopUpRequests)
                .HasForeignKey(f => f.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(tp => tp.PaymentMethods)
                .WithMany(pm => pm.TopUpRequests)
                .HasForeignKey(f => f.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(tp => tp.TransactionId).IsUnique();
        });

        modelBuilder.Entity<AuditLogs>(entity =>
        {
            entity.ToTable("AuditLogs", "dbo", table =>
            {
                table.HasCheckConstraint("CK_AuditLog_Outcome", "[Outcome] in ('SUCCESS', 'REJECTED', 'FAILED')");
            });

            entity.HasKey(a => a.Id);

            entity.Property(a => a.ActorUserId);

            entity.Property(a => a.TransactionId);

            entity.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(60);
            
            entity.Property(a => a.EntityType)
                .HasMaxLength(60)
                .IsRequired();

            entity.Property(a => a.EntityId)
                .HasMaxLength(60);

            entity.Property(a => a.Outcome)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(a => a.Description)
                .HasMaxLength(500);
            
            entity.Property(a => a.CorrelationId)
                .HasMaxLength(100);
            
            entity.Property(a => a.CreatedAtUtc)
                .IsRequired()
                .HasColumnType("datetime");
            
            entity.HasOne(a => a.Users)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(f => f.ActorUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(a => a.Transactions)
                .WithMany(t => t.AuditLogs)
                .HasForeignKey(f => f.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(i => i.ActorUserId);
            entity.HasIndex(i => i.CreatedAtUtc).IsDescending();
        });
    }
}