using Microsoft.EntityFrameworkCore;
using Wallet.Models;

namespace Wallet.Data;

public class LedgerWalletDbContext : DbContext
{
    public DbSet<LedgerWallet> Wallets { get; set; }

    public LedgerWalletDbContext(DbContextOptions options) : base(options)
    {
    }
}
