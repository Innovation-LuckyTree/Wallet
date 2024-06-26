using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;
using WalletService.Domain.Entities;
using WalletService.Domain.Enums;

namespace WalletService.Application.Requests.Accounts.Commands.AddCreditTransaction;

public class AddCreditTransactionCommandHandler : IRequestHandler<AddCreditTransactionCommand, Unit>
{
    private readonly IWalletDbContext _walletDbContext;

    public AddCreditTransactionCommandHandler(IWalletDbContext walletDbContext)
    {
        _walletDbContext = walletDbContext;
    }

    public async Task<Unit> Handle(AddCreditTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await _walletDbContext.Accounts
            .Where(o => o.AccountId == request.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        _ = account ?? throw new EntityNotFoundException("Account", request.AccountId);

        var currentTotalCredits = await _walletDbContext.WalletTransactions
            .Where(o => o.AccountId == request.AccountId)
            .SumAsync(e => e.Amount, cancellationToken);

        var totalBalance = currentTotalCredits + (request.Amount > 0 ? request.Amount * -1 : request.Amount);

        account.Balance = totalBalance;

        var walletTransaction = CreateWalletTransaction(request, totalBalance, currentTotalCredits);

        _walletDbContext.WalletTransactions.Add(walletTransaction);
        _walletDbContext.Accounts.Update(account);

        await _walletDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private WalletTransaction CreateWalletTransaction(AddCreditTransactionCommand request, decimal credit, decimal previosBalance)
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
            ModeOfTransaction = request.ModeOfTransaction
        };
}