using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;
using WalletService.Domain.Entities;

namespace WalletService.Persistence;

public class WalletDbContext : DbContext, IWalletDbContext
{
    public WalletDbContext(DbContextOptions<WalletDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }
    public DbSet<BonusAccount> BonusAccounts { get; set; }
    public DbSet<BonusWalletTransaction> BonusWalletTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "1.1.1-servicing");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WalletDbContext).Assembly);
    }
}
