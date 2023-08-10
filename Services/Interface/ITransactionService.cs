using System.Transactions;
using Wallet.Models;

namespace Wallet.Services
{
    using System;
    using System.Collections.Generic;
    using Wallet.Models;

    public interface ITransactionEventResult
    {
        // Define properties or methods for the result, e.g., Success, Message, Data, etc.
        bool Success { get; }
        string Message { get; }
        object Data { get; } // This can be more specific, e.g., GameTransaction or List<GameTransaction>
    }

    public interface ITransactionEvent
    {
        ITransactionEventResult Result { get; }

        ITransactionEvent Add(GameTransaction transaction);

        ITransactionEvent Show(Guid TransactionID);
        ITransactionEvent Validate();
    }
}
