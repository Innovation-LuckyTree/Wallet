using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;

namespace WalletService.Application.Requests.BonusAccounts.Queries.GetPagedBonusTransactions;

public class GetPagedBonusTransactionsQueryHandler(IWalletDbContext walletDbContext) : IRequestHandler<GetPagedBonusTransactionsQuery, BonusAccountDto>
{
    private readonly IWalletDbContext _walletDbContext = walletDbContext;

    public async Task<BonusAccountDto> Handle(GetPagedBonusTransactionsQuery request, CancellationToken cancellationToken)
    {
        var account = await _walletDbContext.BonusAccounts.Where(o => o.BonusAccountId == request.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        if (account == null)
        {
            return new BonusAccountDto([])
            {
                AccountId = request.AccountId,
                AccountType = "",
                Offset = request.Start + request.PageSize + 1,
                TotalCount = 0
            };
        }

        int totalCount = 0;

        var query = _walletDbContext.BonusWalletTransactions
            .Where(o => o.AccountId == request.AccountId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchKey))
        {
            var searchKeys = request.SearchKey.Split('|');

            query = query.Where(t => searchKeys.Any(e => t.TransactionReference.Contains(e))
                || searchKeys.Any(e => t.TransactionNo.Contains(e))
                || searchKeys.Any(e => t.ModeOfTransaction.Contains(e))
                || searchKeys.Any(e => t.Notes.Contains(e)));
        }

        if (request.PromotionId.HasValue)
            query = query.Where(t => t.PromotionId == request.PromotionId);

        if (request.PromotionStarted.HasValue)
            query = query.Where(t => t.DateStarted >= request.PromotionStarted);

        if (request.ExpirationDate.HasValue)
            query = query.Where(t => t.DateExpired <= request.ExpirationDate);

        if (request.TransactionType.HasValue)
            query = query.Where(t => t.TransactionType == request.TransactionType);

        if (request.StartDate.HasValue)
            query = query.Where(t => t.TransactionDate >= request.StartDate);

        if (request.EndDate.HasValue)
            query = query.Where(t => t.TransactionDate <= request.EndDate);

        totalCount = query.Count();

        query = query.OrderByDescending(o => o.BonusWalletTransactionId);

        query = query.Skip(request.Start);
        query = query.Take(request.PageSize);

        var transactions = await query.Select(o => new BonusAccountTransactionDto
        {
            Id = o.Id,
            TransactionNo = o.TransactionNo,
            TransactionType = o.TransactionType,
            TransactionReference = o.TransactionReference,
            Amount = o.Amount,
            TransactionDate = o.TransactionDate,
            Credit = o.Credit,
            PreviousBalance = o.PreviousBalance,
            ModeOfTransaction = o.ModeOfTransaction,
            Notes = o.Notes,
            PromotionId = o.PromotionId,
            DateStarted = o.DateStarted,
            DateExpired = o.DateExpired
        })
        .ToListAsync(cancellationToken);

        return new BonusAccountDto(transactions)
        {
            AccountId = account.BonusAccountId,
            AccountType = account.BonusAccountType,
            Offset = request.Start + request.PageSize + 1,
            TotalCount = totalCount
        };
    }
}