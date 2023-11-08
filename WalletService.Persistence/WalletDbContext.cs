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

    public DbSet<AccountWalletDoc> AccountWallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountWalletDoc>()
            .Property(b => b.Account)
            .HasColumnType("jsonb");
    }
}
