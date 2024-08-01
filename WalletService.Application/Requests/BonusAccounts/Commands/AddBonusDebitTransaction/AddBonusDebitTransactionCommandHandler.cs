using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;
using WalletService.Application.Requests.BonusAccounts.Commands.CreateBonusAccount;
using WalletService.Domain.Entities;
using WalletService.Domain.Enums;

namespace WalletService.Application.Requests.BonusAccounts.Commands.AddBonusDebitTransaction;

public class AddBonusDebitTransactionCommandHandler(IWalletDbContext walletDbContext, IMediator mediatr) : IRequestHandler<AddBonusDebitTransactionCommand, Unit>
{
    private readonly IWalletDbContext _walletDbContext = walletDbContext;
    private readonly IMediator _mediatr = mediatr;

    public async Task<Unit> Handle(AddBonusDebitTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await _walletDbContext.BonusAccounts
            .Where(o => o.BonusAccountId == request.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        if (account == null)
        {
            var createCommand = new CreateBonusAccountCommand(request.AccountId, request.AccountType,
                request.TransactionNo, request.TransactionReference, request.Amount, request.ModeOfTransaction, request.Notes,
                request.PromotionId, request.DateStarted, request.DateExpired);

            return await _mediatr.Send(createCommand, cancellationToken);
        }

        var currentTotalCredits = await _walletDbContext.BonusWalletTransactions
            .Where(o => o.AccountId == request.AccountId)
            .SumAsync(e => e.Amount, cancellationToken);

        var totalBalance = currentTotalCredits + request.Amount;

        var bonusTransaction = CreateBonusTransaction(request, totalBalance, currentTotalCredits);

        account.Balance = totalBalance;

        _walletDbContext.BonusWalletTransactions.Add(bonusTransaction);

        _walletDbContext.BonusAccounts.Update(account);

        await _walletDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private BonusWalletTransaction CreateBonusTransaction(AddBonusDebitTransactionCommand request, decimal credit, decimal previosBalance) =>
        new ()
        {
            AccountId = request.AccountId,
            TransactionType = TransactionType.Debit,
            TransactionNo = request.TransactionNo,
            TransactionReference = request.TransactionReference,
            Amount = request.Amount,
            Credit = credit,
            PreviousBalance = previosBalance,
            Notes = request.Notes,
            ModeOfTransaction = request.ModeOfTransaction,
            PromotionId = request.PromotionId,
            DateStarted = request.DateStarted,
            DateExpired = request.DateExpired
        };
}