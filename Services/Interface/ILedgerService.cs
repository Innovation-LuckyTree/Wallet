using System.Transactions;
using Wallet.Models;

namespace Wallet.Services.Interface
{
    public interface ILedgerService
    {
        LedgerWallet Wallet { get; }
        public string ToJson(IEnumerable<Transaction> transactions);
        public string Deserialize(string json);
        public decimal CalculateBalance();
        ITransactionEvent CreateNewTransaction();
        ITransactionEvent FilterByTransactionType(GameTransactionType transactionType);
        //only get what's connected in the wallet
        ITransactionEvent GetTransactionById(Guid TransactionID);
    }
}
