using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;

namespace WalletService.Application.Requests.Accounts.Queries.GetAccountTransactions;

public class GetPagedAccountTransactionsQueryHandler(IWalletDbContext walletDbContext) : IRequestHandler<GetPagedAccountTransactionsQuery, AccountDto>
{
    private readonly IWalletDbContext _walletDbContext = walletDbContext;

    public async Task<AccountDto> Handle(GetPagedAccountTransactionsQuery request, CancellationToken cancellationToken)
    {
        int totalCount = 0;

        var accountDoc = await _walletDbContext.AccountWallets.Where(o => o.Id == request.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        var accountTransactions = accountDoc.Account.WalletTransactions;

        if (!string.IsNullOrEmpty(request.SearchKey))
            accountTransactions = accountTransactions.Where(t => t.TransactionReference.Contains(request.SearchKey)
                || t.TransactionNo.Contains(request.SearchKey)
                || t.ModeOfTransaction.Contains(request.SearchKey)
                || t.Notes.Contains(request.SearchKey)).ToList();

        if (request.TransactionType.HasValue)
            accountTransactions = accountTransactions.Where(t => t.TransactionType == request.TransactionType).ToList();

        if (request.StartDate.HasValue)
            accountTransactions = accountTransactions.Where(t => t.TransactionDate >= request.StartDate).ToList();

        if (request.EndDate.HasValue)
            accountTransactions = accountTransactions.Where(t => t.TransactionDate <= request.EndDate).ToList();

        totalCount = accountTransactions.Count();

        var transactions = accountTransactions.OrderByDescending(o => o.TransactionDate).Select(o => new AccountTransactionDto
        {
            Id = o.Id,
            TransactionNo = o.TransactionNo,
            TransactionType = o.TransactionType,
            TransactionReference = o.TransactionReference,
            Amount = o.Amount,
            TransactionDate = o.TransactionDate,
            Credit = o.Credit,
            ModeOfTransaction = o.ModeOfTransaction,
            Notes = o.Notes
        })
        .Skip(request.Start)
        .Take(request.PageSize);

        return new AccountDto(transactions)
        {
            AccountId = accountDoc.Id,
            AccountType = accountDoc.Account.AccountType,
            Offset = request.Start + request.PageSize + 1,
            TotalCount = totalCount
        };
    }
}