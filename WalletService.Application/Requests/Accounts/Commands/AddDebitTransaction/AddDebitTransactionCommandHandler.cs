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
        var account = await _walletDbContext.Accounts
            .Where(o => o.AccountId == request.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        if (account == null)
        {
            var createCommand = new CreateAccountCommand(request.AccountId, request.AccountType,
                request.TransactionNo, request.TransactionReference, request.Amount, request.ModeOfTransaction, request.Notes);

            return await _mediatr.Send(createCommand, cancellationToken);
        }

        var currentTotalCredits = await _walletDbContext.WalletTransactions
            .Where(o => o.AccountId == request.AccountId)
            .SumAsync(e => e.Amount, cancellationToken);

        var totalBalance = currentTotalCredits + request.Amount;

        var walletTransaction = CreateWalletTransaction(request, totalBalance, currentTotalCredits);

        account.Balance = totalBalance;

        _walletDbContext.WalletTransactions.Add(walletTransaction);

        _walletDbContext.Accounts.Update(account);

        await _walletDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private WalletTransaction CreateWalletTransaction(AddDebitTransaction request, decimal credit, decimal previosBalance) =>
    new WalletTransaction
    {
        AccountId = request.AccountId,
        TransactionType = TransactionType.Debit,
        TransactionNo = request.TransactionNo,
        TransactionReference = request.TransactionReference,
        Amount = request.Amount,
        Credit = credit,
        PreviousBalance = previosBalance,
        Notes = request.Notes,
        ModeOfTransaction = request.ModeOfTransaction
    };
}