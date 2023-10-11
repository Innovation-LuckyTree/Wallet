using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using Wallet.Data;
using Wallet.Models;
using Wallet.Services.Interface;

namespace Wallet.Services;


public class TransactionService : ITransactionService
{
    private readonly PaymentTransactionDbContext _dbContext;
    private readonly ITransactionEventResultFactory _result;

    public TransactionService(PaymentTransactionDbContext dbContext, ITransactionEventResultFactory result)
    {
        _dbContext = dbContext;
        _result = result;
    }

    public async Task<ITransactionEventResult> AddAsync(PaymentTransaction transaction)
    {
        ITransactionEventResult result;
        Task task = null; 
        try
        {
            _dbContext.PaymentTransactions.Add(transaction);
            task =  _dbContext.SaveChangesAsync();
            
        }
        catch (Exception ex)
        {
            result = _result.CreateFailureResult($"failed to add transaction{transaction.Id}" +
                $"\n Error:{ex.Message}");
        }

        if (task != null)
            await task;

        result = _result.CreateSuccessResult($"transaction successfully created:{transaction.Id}", transaction);

        return result;
    }

    public async Task<ITransactionEventResult> Exist(Guid TransactionID)
    {
        var transaction = _dbContext.PaymentTransactions.AnyAsync(t => t.Id == TransactionID);
        await transaction;
        if (transaction == null)
            return _result.CreateFailureResult($"Failed to get Transaction{TransactionID}");
        return _result.CreateSuccessResult($"Transaction:{TransactionID} retrieved", transaction);
    }

    public async Task<ITransactionEventResult> ShowAsync(Guid TransactionID)
    {
        var transaction = _dbContext.PaymentTransactions.FirstOrDefaultAsync(t => t.Id == TransactionID);
        await transaction;
        if (transaction == null)
            return _result.CreateFailureResult($"Failed to get Transaction{TransactionID}");
        return _result.CreateSuccessResult($"Transaction:{TransactionID} retrieved", transaction);
    }

    public async Task<ITransactionEventResult> Transactions(Func<IQueryable<PaymentTransaction>, IQueryable<PaymentTransaction>> query)
    {
        var transactions = Task.Factory.StartNew(() => {
            var paymentTransactions = _dbContext.PaymentTransactions;
            var execQuery = query(paymentTransactions);
            return execQuery.ToImmutableList(); });
        await transactions;
        if (transactions.Result == null)
            return _result.CreateFailureResult($"Failed to get queried transactions");
        return _result.CreateSuccessResult($"Transaction:{transactions.Result.Count} retrieved", transactions.Result);
    }

}
