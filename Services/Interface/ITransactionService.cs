namespace Wallet.Services.Interface;

using System;
using Wallet.Models;

public interface IWalletEventResult
{
    public enum Status { Success, Failed, Pending };
    Status isSuccess { get; }
    string? Message { get; }
    object? Data { get; }
}
public interface IWalletEventResultFactory
{
    IWalletEventResult CreateSuccessResult(string message, object data);
    IWalletEventResult CreateFailureResult(string message);
}
public interface ITransactionService
{
    Task<IWalletEventResult> AddAsync(PaymentTransaction transaction);
    Task<IWalletEventResult> ShowAsync(Guid TransactionID);
    Task<IWalletEventResult> Transactions(Func<IQueryable<PaymentTransaction>, IQueryable<PaymentTransaction>> query);
    Task<IWalletEventResult> Exist(Guid TransactionID);

}
