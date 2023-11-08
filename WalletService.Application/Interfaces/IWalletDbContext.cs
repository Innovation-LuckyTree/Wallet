namespace WalletService.Application.Interfaces;

using Microsoft.EntityFrameworkCore;
using WalletService.Domain.Entities;

public interface IWalletDbContext
{
    DbSet<AccountWalletDoc> AccountWallets { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}