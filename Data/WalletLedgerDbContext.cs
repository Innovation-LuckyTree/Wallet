using Microsoft.EntityFrameworkCore;
using Wallet.Models;

namespace Wallet.Data;


public class WalletLedgerDbContext : DbContext
{
    public DbSet<WalletLedger> WalletLedgers { get; set; }

    public WalletLedgerDbContext(DbContextOptions<WalletLedgerDbContext> options)
        : base(options)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
}
