namespace WalletService.Application.Interfaces;

using Microsoft.EntityFrameworkCore;
using WalletService.Domain.Entities;

public interface IWalletDbContext
{
    DbSet<Account> Accounts { get; set; }
    DbSet<WalletTransaction> WalletTransactions { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}