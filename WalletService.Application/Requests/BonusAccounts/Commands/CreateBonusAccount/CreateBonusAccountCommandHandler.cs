using MediatR;
using WalletService.Application.Interfaces;
using WalletService.Domain.Entities;

namespace WalletService.Application.Requests.BonusAccounts.Commands.CreateBonusAccount;

public class CreateBonusAccountCommandHandler(IWalletDbContext walletDbContext) : IRequestHandler<CreateBonusAccountCommand, Unit>
{
    private readonly IWalletDbContext _walletDbContext = walletDbContext;

    public async Task<Unit> Handle(CreateBonusAccountCommand request, CancellationToken cancellationToken)
    {
        var bonusTransaction = CreateBonusTransaction(request, request.Amount);

        var account = new BonusAccount
        {
            BonusAccountId = request.AccountId,
            BonusAccountType = request.AccountType,
            Balance = request.Amount,
            BonusWalletTransactions = [bonusTransaction]
        };

        _walletDbContext.BonusAccounts.Add(account);

        await _walletDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private BonusWalletTransaction CreateBonusTransaction(CreateBonusAccountCommand request, decimal credit)
        => new()
        {
            TransactionNo = request.TransactionNo,
            TransactionReference = request.TransactionReference,
            Amount = request.Amount,
            Notes = request.Notes,
            Credit = credit,
            PreviousBalance = 0,
            ModeOfTransaction = request.ModeOfTransaction,
            PromotionId = request.PromotionId,
            DateStarted = request.DateStarted,
            DateExpired = request.DateExpired
        };
}

