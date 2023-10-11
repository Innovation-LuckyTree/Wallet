using Wallet.Models;

namespace Wallet.Services.Interface;

public interface ILedgerService
{
    Task<decimal> CalculateBalanceAsync(Guid referenceId);
    Task<ITransactionEventResult> CreateNewTransactionAsync(PaymentTransaction transaction);
    Task<ICollection<PaymentTransaction>> FilterByTransactionTypeAsync(Guid referenceId, PaymentTransactionType transactionType);
    Task<PaymentTransaction> GetTransactionByIdAsync(Guid TransactionID);
    Task<ICollection<PaymentTransaction>> FilterByTransactionAsync(Guid referenceId, PaymentTransactionType transactionType, int skip, int take);
}