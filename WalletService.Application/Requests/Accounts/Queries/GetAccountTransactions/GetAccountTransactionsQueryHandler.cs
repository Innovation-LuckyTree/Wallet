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
        var account = await _walletDbContext.Accounts
            .Where(o => o.AccountId == request.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        if (account == null)
        {
            return new AccountDto([])
            {
                AccountId = request.AccountId,
                AccountType = ""
            };
        }

        var accountTransactions = await _walletDbContext.WalletTransactions.Where(o => o.AccountId == request.AccountId)
            .OrderByDescending(o => o.TransactionDate)
            .Select(o => new AccountTransactionDto
            {
                Id = o.Id,
                TransactionNo = o.TransactionNo,
                TransactionType = o.TransactionType,
                TransactionReference = o.TransactionReference,
                Amount = o.Amount,
                TransactionDate = o.TransactionDate,
                Credit = o.Credit,
                PreviousCredit = o.PreviousBalance,
                ModeOfTransaction = o.ModeOfTransaction,
                Notes = o.Notes
            }).ToListAsync(cancellationToken);

        return new AccountDto(accountTransactions)
        {
            AccountId = account.AccountId,
            AccountType = account.AccountType
        };
    }
}