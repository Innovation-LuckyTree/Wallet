using Microsoft.EntityFrameworkCore;
using Wallet.Models;
namespace Wallet.Data;

public class PaymentTransactionDbContext : DbContext
{
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    public PaymentTransactionDbContext(DbContextOptions<PaymentTransactionDbContext> options)
        : base(options)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
}

