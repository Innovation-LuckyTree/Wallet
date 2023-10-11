using Wallet.Models;

namespace Wallet.Services.Interface;

public interface ILedgerService
{
    LedgerWallet Wallet { get; }

    Task<decimal> CalculateBalanceAsync();
    Task<ITransactionEventResult> CreateNewTransactionAsync(PaymentTransaction transaction);
    Task<ICollection<PaymentTransaction>> FilterByTransactionTypeAsync(PaymentTransactionType transactionType);
    Task<PaymentTransaction> GetTransactionByIdAsync(Guid TransactionID);
    Task<ICollection<PaymentTransaction>> FilterByTransactionAsync(PaymentTransactionType transactionType, int skip, int take);
}