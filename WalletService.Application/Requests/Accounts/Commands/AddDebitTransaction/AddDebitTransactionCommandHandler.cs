using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;
using WalletService.Application.Requests.Accounts.Commands.AddTransaction;
using WalletService.Domain.Entities;
using WalletService.Domain.Enums;

namespace WalletService.Application.Requests.Accounts.Commands.AddDebitTransaction;

public class AddDebitTransactionCommandHandler : IRequestHandler<AddDebitTransaction, Unit>
{
    private readonly IWalletDbContext _walletDbContext;
    private readonly IMediator _mediatr;

    public AddDebitTransactionCommandHandler(IWalletDbContext walletDbContext, IMediator mediatr)
    {
        _walletDbContext = walletDbContext;
        _mediatr = mediatr;
    }

    public async Task<Unit> Handle(AddDebitTransaction request, CancellationToken cancellationToken)
    {
        var accountWalletDoc = await _walletDbContext.AccountWallets
            .Where(o => o.Id == request.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        if (accountWalletDoc == null)
        {
            var createCommand = new CreateAccountCommand(request.AccountId, request.AccountType,
                request.TransactionNo, request.TransactionReference, request.Amount, request.Notes);

            return await _mediatr.Send(createCommand, cancellationToken);
        }

        var totalBalance = accountWalletDoc.Account.WalletTransactions.Sum(o => o.Amount) + request.Amount;
        
        var walletTransaction = CreateWalletTransaction(request, totalBalance);

        accountWalletDoc.Account.Balance = totalBalance;

        accountWalletDoc.Account.WalletTransactions.Add(walletTransaction);

        _walletDbContext.AccountWallets.Update(accountWalletDoc);

        await _walletDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private WalletTransaction CreateWalletTransaction(AddDebitTransaction request, decimal credit) =>
    new WalletTransaction
    {
        TransactionType = TransactionType.Debit,
        TransactionNo = request.TransactionNo,
        TransactionReference = request.TransactionReference,
        Amount = request.Amount,
        Credit = credit,
        Notes = request.Notes,
    };
}