using Wallet.Models;
using Wallet.RequestModel;

namespace Wallet.Services.Interface;

public interface ILedgerService
{
    Task<decimal> CalculateBalanceAsync(Guid accountId);
    Task<IWalletEventResult> CreateNewTransactionAsync(WalletLedger walletLedger);
    Task<ICollection<WalletLedger>> FilterByTransactionTypeAsync(Guid accountId, string transactionType);
    Task<ICollection<WalletLedger>> GetTransaction(TransactionRequestModel transactionRequest);
    Task<WalletLedger> GetTransactionByTransactionNoAsync(string transactionNo);
    Task<WalletLedger> GetTransactionByAccountIdAsync(Guid accountId);
    Task<ICollection<WalletLedger>> FilterByTransactionAsync(Guid accountId, string transactionType, int skip, int take);
}