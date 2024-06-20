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

        var account = new Account
        {
            AccountId = request.AccountId,
            AccountType = request.AccountType,
            Balance = request.Amount,
            WalletTransactions = [walletTransaction]
        };

        _walletDbContext.Accounts.Add(account);

        await _walletDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private WalletTransaction CreateWalletTransaction(CreateAccountCommand request, decimal credit)
        => new()
        {
            TransactionNo = request.TransactionNo,
            TransactionReference = request.TransactionReference,
            Amount = request.Amount,
            Notes = request.Notes,
            Credit = credit,
            PreviousBalance = 0,
            ModeOfTransaction = request.ModeOfTransaction
        };
}