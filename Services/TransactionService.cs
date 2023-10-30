using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Wallet.Data;
using Wallet.Models;
using Wallet.Services.Interface;

namespace Wallet.Services;


public class TransactionService : ITransactionService
{
    private readonly WalletLedgerDbContext _dbContext;
    private readonly IWalletEventResultFactory _result;
    private readonly WalletLedgerValidator _validator;

    public TransactionService(WalletLedgerDbContext dbContext, IWalletEventResultFactory result, WalletLedgerValidator validator)
    {
        _dbContext = dbContext;
        _result = result;
        _validator = validator;
    }

    public async Task<IWalletEventResult> AddAsync(WalletLedger walletLedger)
    {
        IWalletEventResult result;
        Task task = null; 
        try
        {
            if (!_validator.Validate(walletLedger).IsValid)
                throw new Exception(nameof(walletLedger));
            _dbContext.WalletLedgers.Add(walletLedger);
            task =  _dbContext.SaveChangesAsync();
            
        }
        catch (Exception ex)
        {
            result = _result.CreateFailureResult($"failed to add transaction{walletLedger.Id}" +
                $"\n Error:{ex.Message}");
            return result;
        }

        if (task != null)
            await task;

        result = _result.CreateSuccessResult($"transaction successfully created:{walletLedger.Id}", walletLedger);

        return result;
    }

    public async Task<IWalletEventResult> Exist(Guid TransactionID)
    {
        var transaction = _dbContext.WalletLedgers.AnyAsync(t => t.Id == TransactionID);
        await transaction;
        if (transaction == null)
            return _result.CreateFailureResult($"Failed to get Transaction{TransactionID}");
        return _result.CreateSuccessResult($"Transaction:{TransactionID} retrieved", transaction);
    }

    public async Task<IWalletEventResult> ShowAsync(Guid TransactionID)
    {
        var transaction = _dbContext.WalletLedgers.FirstOrDefaultAsync(t => t.Id == TransactionID);
        await transaction;
        if (transaction == null)
            return _result.CreateFailureResult($"Failed to get Transaction{TransactionID}");
        return _result.CreateSuccessResult($"Transaction:{TransactionID} retrieved", transaction);
    }

    public async Task<IWalletEventResult> Transactions(Func<IQueryable<WalletLedger>, IQueryable<WalletLedger>> query)
    {
        var transactions = Task.Factory.StartNew(() => {
            var walletLedgers = _dbContext.WalletLedgers;
            var execQuery = query(walletLedgers);
            return execQuery.ToImmutableList(); });
        await transactions;
        if (transactions.Result == null)
            return _result.CreateFailureResult($"Failed to get queried transactions");
        return _result.CreateSuccessResult($"Transaction:{transactions.Result.Count} retrieved", transactions.Result);
    }

}
