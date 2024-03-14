using MediatR;
using WalletService.Application.Interfaces;
using WalletService.Domain.Entities;

namespace WalletService.Application.Requests.Accounts.Commands.AddTransaction;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Unit>
{
    private readonly IWalletDbContext _walletDbContext;

    public CreateAccountCommandHandler(IWalletDbContext walletDbContext)
    {
        _walletDbContext = walletDbContext;
    }

    public async Task<Unit> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var walletTransaction = CreateWalletTransaction(request, request.Amount);

        var accountWalletDoc = new AccountWalletDoc
        {
            Id = request.AccountId,
            Account = new Account
            {
                AccountId = request.AccountId,
                AccountType = request.AccountType,
                Balance = request.Amount,
                WalletTransactions = new[] { walletTransaction }
            }
        };

        _walletDbContext.AccountWallets.Add(accountWalletDoc);

        await _walletDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private WalletTransaction CreateWalletTransaction(CreateAccountCommand request, decimal credit) =>
        new WalletTransaction
        {
            TransactionNo = request.TransactionNo,
            TransactionReference = request.TransactionReference,
            Amount = request.Amount,
            Notes = request.Notes,
            Credit = credit,
            ModeOfTransaction = request.ModeOfTransaction
        };
}