using System.Transactions;
using Wallet.Models;

namespace Wallet.Services
{
    using Microsoft.AspNetCore.Components.Web;
    using System;
    using System.Collections.Generic;
    using Wallet.Models;

    public interface ITransactionEventResult
    {
        // Define properties or methods for the result, e.g., Success, Message, Data, etc.
        string? Message { get; }
        object? Data { get; } // This can be more specific, e.g., GameTransaction or List<GameTransaction>
    }
    public interface ITransactionEventResultFactory
    {
        ITransactionEventResult CreateSuccessResult(string message, object data);
        ITransactionEventResult CreateFailureResult(string message);
    }
    public interface ITransactionEvent
    {
        Task<ITransactionEventResult> AddAsync(PaymentTransaction transaction);
        Task<ITransactionEventResult> ShowAsync(Guid TransactionID);
        Task<ITransactionEventResult> Transactions(Func<PaymentTransaction, bool> query);
        Task<ITransactionEventResult> Exist(Guid TransactionID);
    }
}
