using Microsoft.EntityFrameworkCore;
using Wallet.Data;
using Wallet.Models;
using Wallet.Services.Factory;
using Wallet.Services.Interface;

namespace Wallet.Services;

public class LedgerService : ILedgerService
{
    private readonly LedgerWalletDbContext _ledgerWalletDb;
    private readonly ITransactionService _transactionEvent;
    private readonly IWalletEventResultFactory _walletEventResultFactory;

    public LedgerService(LedgerWalletDbContext ledgerWalletDb, ITransactionService transactionEvent, IWalletEventResultFactory walletEventResultFactory)
    {
        _ledgerWalletDb = ledgerWalletDb;
        _transactionEvent = transactionEvent;
        _walletEventResultFactory = walletEventResultFactory;
    }
    private static void Validate(IWalletEventResult result)
    {
        if (result == null)
            throw new ArgumentNullException(nameof(result));
        if (result.isSuccess == IWalletEventResult.Status.Failed)
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
    public async Task<decimal> CalculateBalanceAsync(Guid referenceId)
    {
        var transactionResult = await _transactionEvent.Transactions(query => query.Where(x => x.ReferenceId == referenceId));
        Validate((WalletEventResult)transactionResult);

        var transactions = toPaymentTransactions(transactionResult);

        return transactions.Sum(transaction =>
            transaction.TransactionType == PaymentTransactionType.Credit ? transaction.Amount : -transaction.Amount
        );
    }

    public async Task<IWalletEventResult> CreateNewTransactionAsync(PaymentTransaction transaction)
    {
        var transactionResult = await _transactionEvent.AddAsync(transaction);
        Validate((WalletEventResult)transactionResult);
        return transactionResult;
    }
    public async Task<ICollection<PaymentTransaction>> FilterByTransactionAsync(Guid referenceId, PaymentTransactionType transactionType, int skip, int take)
    {
        var transactionResult = await _transactionEvent.Transactions(query =>
            query.Where(x => x.TransactionType == transactionType && x.ReferenceId == referenceId)
            .Skip(skip).Take(take));
        return toPaymentTransactions((WalletEventResult)transactionResult);
    }
    public async Task<ICollection<PaymentTransaction>> FilterByTransactionTypeAsync(Guid referenceId, PaymentTransactionType transactionType)
    {
        var transactionResult = await _transactionEvent.Transactions(query => query.
            Where(x => x.TransactionType == transactionType && x.ReferenceId == referenceId));
        return toPaymentTransactions((WalletEventResult)transactionResult);
    }

    public async Task<PaymentTransaction> GetTransactionByIdAsync(Guid TransactionID)
    {
        var query = await _transactionEvent.ShowAsync(TransactionID);
        if (query is not PaymentTransaction result)
            throw new Exception("Invalid transaction data type");
        return result;
    }

    public async Task<IWalletEventResult> GetLedgerWalletAsync(Guid Id)
    {
        IWalletEventResult? result = null;
        try
        {
            var wallet = await _ledgerWalletDb.Wallets.FirstOrDefaultAsync(x => x.Id == Id);
            if (wallet != null)
             result =  _walletEventResultFactory.CreateSuccessResult($"wallet{Id} found", wallet);
        }
        catch (Exception ex)
        {
          result =  _walletEventResultFactory.CreateFailureResult("failed to find wallet");
        }

        return result;
    }

    public async Task<IWalletEventResult> CreateWallet(string name)
    {
        IWalletEventResult? result = null;
        try
        {
            var wallet = new LedgerWallet();
            wallet.Created = DateTime.UtcNow;
            wallet.CreatedBy = name;
            _ledgerWalletDb.Add(wallet);
            var isAdded = await _ledgerWalletDb.SaveChangesAsync();
            if (isAdded == 1)
                result = _walletEventResultFactory.CreateSuccessResult("Successfully created", wallet);

        }
        catch (Exception ex)
        {
          result = _walletEventResultFactory.CreateFailureResult($"failed to create wallet:{ex.Message}");
        }
        return result;
    }
    public async Task DeleteLedgerWallet(Guid id)
    {
        var existingWallet = await GetLedgerWalletAsync(id);
        if (existingWallet != null)
        {
            ledgerWallets.Remove(existingWallet);
        }
    }

}