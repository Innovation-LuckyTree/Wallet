using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;
using WalletService.Domain.Entities;
using WalletService.Domain.Enums;

namespace WalletService.Application.Requests.Accounts.Commands.AddCreditTransaction;

public class AddCreditTransactionCommandHandler : IRequestHandler<AddCreditTransactionCommand, Unit>
{
    private readonly IWalletDbContext _walletDbContext;

    public AddCreditTransactionCommandHandler(IWalletDbContext walletDbContext, IMediator mediatr)
    {
        _walletDbContext = walletDbContext;
    }

    public async Task<Unit> Handle(AddCreditTransactionCommand request, CancellationToken cancellationToken)
    {
        var accountWalletDoc = await _walletDbContext.AccountWallets
            .Where(o => o.Id == request.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        _ = accountWalletDoc ?? throw new EntityNotFoundException("Account", request.AccountId);

        var totalBalance = accountWalletDoc.Account.WalletTransactions.Sum(o => o.Amount) + (request.Amount > 0 ? request.Amount * -1 : request.Amount);
        accountWalletDoc.Account.Balance = totalBalance;

        var walletTransaction = CreateWalletTransaction(request, totalBalance);

        accountWalletDoc.Account.WalletTransactions.Add(walletTransaction);

        _walletDbContext.AccountWallets.Update(accountWalletDoc);

        await _walletDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private WalletTransaction CreateWalletTransaction(AddCreditTransactionCommand request, decimal credit) =>
        new WalletTransaction
        {
            TransactionNo = request.TransactionNo,
            TransactionType = TransactionType.Credit,
            TransactionReference = request.TransactionReference,
            Amount = request.Amount > 0 ? request.Amount * -1 : request.Amount,
            Credit = credit,
            Notes = request.Notes,
        };
}