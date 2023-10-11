using Wallet.Models;
using Wallet.Services.Factory;
using Wallet.Services.Interface;

namespace Wallet.Services;

public class LedgerService :  ILedgerService
{
    public LedgerWallet Wallet { get; private set; }

    private readonly ITransactionService _transactionEvent;

    public LedgerService(LedgerWallet wallet, ITransactionService transactionEvent)
    {
        Wallet = wallet;
        _transactionEvent = transactionEvent;
    }
    private static void Validate(TransactionEventResult result)
    {
        if (result == null)
            throw new ArgumentNullException(nameof(result));
        if (result.Success == TransactionEventResult.Status.Failed)
            throw new InvalidOperationException(result.Message);

    }
    private static ICollection<PaymentTransaction> toPaymentTransactions(object data)
    {
        if (data is not ICollection<PaymentTransaction> resultList)
        {
            throw new Exception("Invalid transaction data type");
        }

        if (resultList.Count == 0)
        {
            throw new Exception("No Transaction found");
        }
        return resultList;
    }
    public async Task<decimal> CalculateBalanceAsync()
    {
        var transactionResult = await _transactionEvent.Transactions(query => query.Where(x => x.ReferenceId == Wallet.ReferenceID));
        Validate((TransactionEventResult)transactionResult);

        var transactions = toPaymentTransactions(transactionResult);

        return transactions.Sum(transaction =>
            transaction.TransactionType == PaymentTransactionType.Credit ? transaction.Amount : -transaction.Amount
        );
    }

    public async Task<ITransactionEventResult> CreateNewTransactionAsync(PaymentTransaction transaction)
    {
        var transactionResult = await _transactionEvent.AddAsync(transaction);
        Validate((TransactionEventResult)transactionResult);
        return transactionResult;
    }
    public async Task<ICollection<PaymentTransaction>> FilterByTransactionAsync(PaymentTransactionType transactionType, int skip, int take)
    {
        var transactionResult = await _transactionEvent.Transactions(query => 
            query.Where(x=>  x.TransactionType == transactionType && x.ReferenceId == Wallet.ReferenceID)
            .Skip(skip).Take(take));
        return toPaymentTransactions((TransactionEventResult)transactionResult);
    }
    public async Task<ICollection<PaymentTransaction>> FilterByTransactionTypeAsync(PaymentTransactionType transactionType)
    {
        var transactionResult = await _transactionEvent.Transactions(query => query.
            Where(x => x.TransactionType == transactionType && x.ReferenceId == Wallet.ReferenceID));
        return toPaymentTransactions((TransactionEventResult)transactionResult);
    }

    public async Task<PaymentTransaction> GetTransactionByIdAsync(Guid TransactionID)
    {
        var query = await _transactionEvent.ShowAsync(TransactionID);
        if (query is not PaymentTransaction result)
            throw new Exception("Invalid transaction data type");
        return result;
    }
}