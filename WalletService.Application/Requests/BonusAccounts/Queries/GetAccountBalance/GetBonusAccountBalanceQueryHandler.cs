using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;

namespace WalletService.Application.Requests.BonusAccounts.Queries.GetBonusAccountBalance;

public class GetBonusAccountBalanceQueryHandler(IWalletDbContext walletDbContext) : IRequestHandler<GetBonusAccountBalanceQuery, BonusAccountBalance>
{
    private readonly IWalletDbContext _walletDbContext = walletDbContext;

    public async Task<BonusAccountBalance> Handle(GetBonusAccountBalanceQuery request, CancellationToken cancellationToken)
    {
        var bonusAccountBalance = await _walletDbContext.BonusAccounts
            .Where(o => o.BonusAccountId == request.AccountId)
            .Select(o => new BonusAccountBalance
            {
                AccountId = o.BonusAccountId,
                AccountType = o.BonusAccountType,
                Balance = o.Balance
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (bonusAccountBalance == null)
        {
            return new BonusAccountBalance
            {
                AccountId = request.AccountId,
                AccountType = "",
                Balance = 0
            };
        }
        var dNow = DateTime.Now;

        var bonusTransactions = await _walletDbContext.BonusWalletTransactions
            .Where(o => o.AccountId == bonusAccountBalance.AccountId && o.DateStarted >= dNow && o.DateExpired <= dNow)
            .GroupBy(o => o.PromotionId)
                .Select(o => new PromotionDetail
                {
                    PromotionId = o.Key,
                    RemainingAmount = o.Sum(e => e.Amount),
                    DateStarted = o.Min(e => e.DateStarted),
                    ExpirationDate = o.Min(e => e.DateExpired),
                    ConsumedAmount = o.Where(e => e.TransactionType == 0).Sum(e => e.Amount)
                })
            .ToListAsync(cancellationToken);

        bonusAccountBalance.PromotionDetails = bonusTransactions;

        return bonusAccountBalance;
    }
}
