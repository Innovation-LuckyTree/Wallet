using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;

namespace WalletService.Application.Requests.BonusAccounts.Queries.GetBonusPromotionBalance;

public class GetBonusPromotionBalanceQueryHandler(IWalletDbContext walletDbContext) : IRequestHandler<GetBonusPromotionBalanceQuery, BonusAccountVm>
{
    private readonly IWalletDbContext _walletDbContext = walletDbContext;

    public async Task<BonusAccountVm> Handle(GetBonusPromotionBalanceQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _walletDbContext.BonusWalletTransactions
            .Where(o => o.AccountId == request.AccountId && o.PromotionId == request.PromotionId
                && o.DateStarted.Date == request.DateExpired.Date && o.DateExpired.Date == request.DateExpired.Date)
            .Select(o => new BonusAccountTransactionDto
            {
                Id = o.Id,
                AccountId = o.AccountId,
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

        // get credited bonus. 0 - for debit
        var creditPromotion = transactions.First(o => o.TransactionType == 0);
        var remainingBalance = transactions.Sum(o => o.Amount);
        var consumedCreditBonus = transactions.Where(o => o.TransactionType != 0).Sum(o => o.Amount);

        return new BonusAccountVm
        {
            AccountId = creditPromotion.AccountId,
            PromotionId = creditPromotion.PromotionId.Value,
            DateStart = creditPromotion.DateStarted.Value,
            DateExpired = creditPromotion.DateExpired.Value,
            ConsumedAmount = consumedCreditBonus,
            Balance = remainingBalance,
            AccountTransactions = transactions
        };
    }
}