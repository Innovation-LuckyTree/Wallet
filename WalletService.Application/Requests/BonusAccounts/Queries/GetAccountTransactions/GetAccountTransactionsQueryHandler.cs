using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;

namespace WalletService.Application.Requests.BonusAccounts.Queries.GetAccountTransactions;

public class GetAccountTransactionsQueryHandler(IWalletDbContext walletDbContext) : IRequestHandler<GetAccountTransactionsQuery, BonusAccountDto>
{
    private readonly IWalletDbContext _walletDbContext = walletDbContext;

    public async Task<BonusAccountDto> Handle(GetAccountTransactionsQuery request, CancellationToken cancellationToken)
    {
        var account = await _walletDbContext.BonusAccounts
            .Where(o => o.BonusAccountId == request.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        if (account == null)
        {
            return new BonusAccountDto([])
            {
                AccountId = request.AccountId,
                AccountType = ""
            };
        }

        var accountTransactions = await _walletDbContext.BonusWalletTransactions.Where(o => o.AccountId == request.AccountId)
            .OrderByDescending(o => o.TransactionDate)
            .Select(o => new BonusAccountTransactionDto
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
            }).ToListAsync(cancellationToken);

        return new BonusAccountDto(accountTransactions)
        {
            AccountId = account.BonusAccountId,
            AccountType = account.BonusAccountType
        };
    }
}