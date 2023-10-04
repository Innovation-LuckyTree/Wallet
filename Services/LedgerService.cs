using System.Transactions;
using Wallet.Models;

namespace Wallet.Services.Interface;

public class LedgerService : ILedgerService
{
    public LedgerWallet Wallet => throw new NotImplementedException();

    public decimal CalculateBalance()
    {
        throw new NotImplementedException();
    }

    public ITransactionEvent CreateNewTransaction()
    {
        throw new NotImplementedException();
    }

    public string Deserialize(string json)
    {
        throw new NotImplementedException();
    }

    public ITransactionEvent FilterByTransactionType(PaymentTransactionType transactionType)
    {
        throw new NotImplementedException();
    }

    public ITransactionEvent GetTransactionById(Guid TransactionID)
    {
        throw new NotImplementedException();
    }

    public string ToJson(IEnumerable<Transaction> transactions)
    {
        throw new NotImplementedException();
    }
}

