using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;
using WalletService.Domain.Enums;

namespace WalletService.Application.Requests.BonusAccounts.Queries.GetBonusAccountBalance;

public class GetBonusAccountBalanceQueryHandler(IWalletDbContext walletDbContext) : IRequestHandler<GetBonusAccountBalanceQuery, BonusAccountBalance>
{
    private readonly IWalletDbContext _walletDbContext = walletDbContext;

    public async Task<BonusAccountBalance> Handle(GetBonusAccountBalanceQuery request, CancellationToken cancellationToken)
    {
        var dNow = DateTime.Now;

        var bonusPromotions = await _walletDbContext.BonusWalletTransactions
            .Where(o => o.AccountId == request.AccountId && o.DateStarted.Date <= dNow.Date && o.DateExpired.Date >= dNow.Date)
            .GroupBy(o => new { o.AccountId, o.BonusAccount.BonusAccountType, o.PromotionId, o.DateExpired, o.DateStarted })
            .Select(o => new PromotionDetail
            {
                AccountId = o.Key.AccountId,
                AccountType = o.Key.BonusAccountType,
                PromotionId = o.Key.PromotionId,
                RemainingAmount = o.Sum(e => e.Amount),
                DateStarted = o.Key.DateStarted,
                ExpirationDate = o.Key.DateExpired,
                ConsumedAmount = o.Where(e => e.TransactionType == TransactionType.Credit).Sum(e => e.Amount)
            })
            .ToListAsync(cancellationToken);

        if ((bonusPromotions?.Count() ?? 0) == 0)
        {
            return new BonusAccountBalance
            {
                AccountId = request.AccountId,
                AccountType = "",
                Balance = 0
            };
        }

        return new BonusAccountBalance
        {
            AccountId = request.AccountId,
            AccountType = bonusPromotions.First().AccountType,
            Balance = bonusPromotions.Sum(o => o.RemainingAmount),
            PromotionDetails = bonusPromotions
        }; ;
    }
}
