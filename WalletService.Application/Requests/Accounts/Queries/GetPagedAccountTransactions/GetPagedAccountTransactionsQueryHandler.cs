using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;

namespace WalletService.Application.Requests.Accounts.Queries.GetAccountTransactions;

public class GetPagedAccountTransactionsQueryHandler(IWalletDbContext walletDbContext) : IRequestHandler<GetPagedAccountTransactionsQuery, AccountDto>
{
    private readonly IWalletDbContext _walletDbContext = walletDbContext;

    public async Task<AccountDto> Handle(GetPagedAccountTransactionsQuery request, CancellationToken cancellationToken)
    {
        var account = await _walletDbContext.Accounts.Where(o => o.AccountId == request.AccountId).FirstOrDefaultAsync(cancellationToken);

        if (account == null)
        {
            return new AccountDto([])
            {
                AccountId = request.AccountId,
                AccountType = "",
                Offset = request.Start + request.PageSize + 1,
                TotalCount = 0
            };
        }

        int totalCount = 0;

        var query = _walletDbContext.WalletTransactions
            .Where(o => o.AccountId == request.AccountId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchKey))
            query = query.Where(t => t.TransactionReference.Contains(request.SearchKey)
                || t.TransactionNo.Contains(request.SearchKey)
                || t.ModeOfTransaction.Contains(request.SearchKey)
                || t.Notes.Contains(request.SearchKey));

        if (request.TransactionType.HasValue)
            query = query.Where(t => t.TransactionType == request.TransactionType);

        if (request.StartDate.HasValue)
            query = query.Where(t => t.TransactionDate >= request.StartDate);

        if (request.EndDate.HasValue)
            query = query.Where(t => t.TransactionDate <= request.EndDate);

        totalCount = query.Count();

        query = query.OrderByDescending(o => o.WalletTransactionId);

        query = query.Skip(request.Start);
        query = query.Take(request.PageSize);

        var transactions = await query.Select(o => new AccountTransactionDto
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

        return new AccountDto(transactions)
        {
            AccountId = account.AccountId,
            AccountType = account.AccountType,
            Offset = request.Start + request.PageSize + 1,
            TotalCount = totalCount
        };
    }
}