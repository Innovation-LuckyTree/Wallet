using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;

namespace WalletService.Application.Requests.Accounts.Queries.GetAccountTransactions;

public class GetAccountTransactionsQueryHandler : IRequestHandler<GetAccountTransactionsQuery, AccountDto>
{
    private readonly IWalletDbContext _walletDbContext;

    public GetAccountTransactionsQueryHandler(IWalletDbContext walletDbContext)
    {
        _walletDbContext = walletDbContext;
    }

    public async Task<AccountDto> Handle(GetAccountTransactionsQuery request, CancellationToken cancellationToken)
    {
        var accountDoc = await _walletDbContext.AccountWallets.Where(o => o.Id == request.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        var transactions = accountDoc.Account.WalletTransactions.Select(o => new AccountTransactionDto
        {
            Id = o.Id,
            TransactionNo = o.TransactionNo,
            TransactionType = o.TransactionType,
            TransactionReference = o.TransactionReference,
            Amount = o.Amount,
            TransactionDate = o.TransactionDate,
            Credit = o.Credit,
            ModeOfTransaction = o.ModeOfTransaction,
            Notes = o.Notes
        });

        return new AccountDto(transactions)
        {
            AccountId = accountDoc.Id,
            AccountType = accountDoc.Account.AccountType
        };
    }
}