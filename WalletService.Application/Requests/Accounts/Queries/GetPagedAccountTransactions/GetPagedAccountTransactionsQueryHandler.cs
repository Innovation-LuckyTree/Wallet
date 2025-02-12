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

        var summary = await query.GroupBy(t => 1)
            .Select(g => new
            {
                TotalCount = g.Count(),
                TotalDebit = g.Where(t => t.TransactionType == TransactionType.Debit).Sum(t => (decimal?)t.Amount) ?? 0,
                TotalCredit = g.Where(t => t.TransactionType == TransactionType.Credit).Sum(t => (decimal?)t.Amount) * -1 ?? 0,
                DebitTransactionCount = g.Count(t => t.TransactionType == TransactionType.Debit),
                CreditsTransactionCount = g.Count(t => t.TransactionType == TransactionType.Credit)
            })
            .FirstOrDefaultAsync(cancellationToken);

        var totalCount = summary?.TotalCount ?? 0;
        var totalDebit = summary?.TotalDebit ?? 0;
        var totalCredit = summary?.TotalCredit ?? 0;
        var debitTransactionCount = summary?.DebitTransactionCount ?? 0;
        var creditsTransactionCount = summary?.CreditsTransactionCount ?? 0;

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