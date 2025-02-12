using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;
using WalletService.Domain.Enums;

namespace WalletService.Application.Requests.Accounts.Queries.GetAccountTransactions;

public class GetPagedAccountTransactionsQueryHandler(IWalletDbContext walletDbContext) : IRequestHandler<GetPagedAccountTransactionsQuery, TransactionsAccountDto>
{
    private readonly IWalletDbContext _walletDbContext = walletDbContext;

    public async Task<TransactionsAccountDto> Handle(GetPagedAccountTransactionsQuery request, CancellationToken cancellationToken)
    {
        var account = await _walletDbContext.Accounts.Where(o => o.AccountId == request.AccountId).FirstOrDefaultAsync(cancellationToken);

        if (account == null)
        {
            return new TransactionsAccountDto([], 0, 0, 0, 0, 0)
            {
                AccountId = request.AccountId,
                AccountType = "",
                Offset = request.Start + request.PageSize + 1
            };
        }

        var query = _walletDbContext.WalletTransactions
            .Where(o => o.AccountId == request.AccountId);

        if (!string.IsNullOrEmpty(request.SearchKey))
        {
            var searchKeys = request.SearchKey.Split('|');

            query = query.Where(t => searchKeys.Any(e => t.TransactionReference.Contains(e))
                || searchKeys.Any(e => t.TransactionNo.Contains(e))
                || searchKeys.Any(e => t.ModeOfTransaction.Contains(e))
                || searchKeys.Any(e => t.Notes.Contains(e)));
        }

        if (request.TransactionType.HasValue)
            query = query.Where(t => t.TransactionType == request.TransactionType);

        if (request.StartDate.HasValue)
            query = query.Where(t => t.TransactionDate >= request.StartDate);

        if (request.EndDate.HasValue)
            query = query.Where(t => t.TransactionDate <= request.EndDate);

        var totalCount = await query.CountAsync(cancellationToken);
        var totalDebit = await query.Where(t => t.TransactionType == TransactionType.Debit).SumAsync(t => (decimal?)t.Amount, cancellationToken: cancellationToken) ?? 0;
        var totalCredit = await query.Where(t => t.TransactionType == TransactionType.Credit).SumAsync(t => (decimal?)t.Amount, cancellationToken: cancellationToken)  * -1 ?? 0;
        var debitTransactionCount = await query.CountAsync(t => t.TransactionType == TransactionType.Debit, cancellationToken);
        var creditsTransactionCount = await query.CountAsync(t => t.TransactionType == TransactionType.Credit, cancellationToken);

        var transactions = await query
            .OrderByDescending(o => o.WalletTransactionId)
            .Skip(request.Start)
            .Take(request.PageSize)
            .Select(o => new AccountTransactionDto
            {
                Id = o.Id,
                TransactionNo = o.TransactionNo,
                TransactionType = o.TransactionType,
                TransactionReference = o.TransactionReference,
                Amount = o.Amount,
                TransactionDate = o.TransactionDate,
                Credit = o.Credit,
                PreviousCredit = o.PreviousBalance,
                ModeOfTransaction = o.ModeOfTransaction,
                Notes = o.Notes
            })
            .ToListAsync(cancellationToken);

        return new TransactionsAccountDto(transactions, totalCount, totalDebit, totalCredit, debitTransactionCount, creditsTransactionCount)
        {
            AccountId = account.AccountId,
            AccountType = account.AccountType,
            Offset = request.Start + request.PageSize + 1
        };
    }
}