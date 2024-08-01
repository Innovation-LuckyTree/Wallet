using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;
using WalletService.Domain.Entities;
using WalletService.Domain.Enums;

namespace WalletService.Application.Requests.BonusAccounts.Commands.AddBonusCreditTransaction;

public class AddBonusCreditTransactionCommandHandler(IWalletDbContext walletDbContext) : IRequestHandler<AddBonusCreditTransactionCommand, Unit>
{
    private readonly IWalletDbContext _walletDbContext = walletDbContext;

    public async Task<Unit> Handle(AddBonusCreditTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await _walletDbContext.BonusAccounts
            .Where(o => o.BonusAccountId == request.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        _ = account ?? throw new EntityNotFoundException("Account", request.AccountId);

        var currentTotalCredits = await _walletDbContext.BonusWalletTransactions
            .Where(o => o.AccountId == request.AccountId)
            .SumAsync(e => e.Amount, cancellationToken);

        var totalBalance = currentTotalCredits + (request.Amount > 0 ? request.Amount * -1 : request.Amount);

        account.Balance = totalBalance;

        var bonusTransaction = CreateBonusTransaction(request, totalBalance, currentTotalCredits);

        _walletDbContext.BonusWalletTransactions.Add(bonusTransaction);
        _walletDbContext.BonusAccounts.Update(account);

        await _walletDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private BonusWalletTransaction CreateBonusTransaction(AddBonusCreditTransactionCommand request, decimal credit, decimal previosBalance)
        => new()
        {
            AccountId = request.AccountId,
            TransactionNo = request.TransactionNo,
            TransactionType = TransactionType.Credit,
            TransactionReference = request.TransactionReference,
            Amount = request.Amount > 0 ? request.Amount * -1 : request.Amount,
            Credit = credit,
            PreviousBalance = previosBalance,
            Notes = request.Notes,
            ModeOfTransaction = request.ModeOfTransaction,
            PromotionId = request.PromotionId,
            DateStarted = request.DateStarted,
            DateExpired = request.DateExpired
        };
}