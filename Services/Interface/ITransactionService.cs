namespace Wallet.Services.Interface;

using System;
using Wallet.Models;

public interface ITransactionEventResult
{
    string? Message { get; }
    object? Data { get; }
}
public interface ITransactionEventResultFactory
{
    ITransactionEventResult CreateSuccessResult(string message, object data);
    ITransactionEventResult CreateFailureResult(string message);
}
public interface ITransactionService
{
    Task<ITransactionEventResult> AddAsync(PaymentTransaction transaction);
    Task<ITransactionEventResult> ShowAsync(Guid TransactionID);
    Task<ITransactionEventResult> Transactions(Func<IQueryable<PaymentTransaction>, IQueryable<PaymentTransaction>> query);
    Task<ITransactionEventResult> Exist(Guid TransactionID);

}
