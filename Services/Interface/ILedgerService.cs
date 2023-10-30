using Wallet.Models;

namespace Wallet.Services.Interface;

public interface ILedgerService
{
    Task<decimal> CalculateBalanceAsync(Guid accountId);
    Task<IWalletEventResult> CreateNewTransactionAsync(WalletLedger walletLedger);
    Task<ICollection<WalletLedger>> FilterByTransactionTypeAsync(Guid accountId, string transactionType);
    Task<WalletLedger> GetTransactionByIdAsync(Guid TransactionID);
    Task<ICollection<WalletLedger>> FilterByTransactionAsync(Guid accountId, string transactionType, int skip, int take);
}