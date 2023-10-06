using Microsoft.EntityFrameworkCore;
using Wallet.Models;

namespace Wallet.Data
{
    public class PaymentTransactionDbContext : DbContext
    {
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Your_Connection_String_Here");
        }
    }
}
