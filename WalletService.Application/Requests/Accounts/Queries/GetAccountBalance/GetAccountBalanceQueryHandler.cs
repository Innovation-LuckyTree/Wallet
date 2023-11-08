using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;

namespace WalletService.Application.Requests.Accounts.Queries.GetAccountBalance;

public class GetAccountBalanceQueryHandler : IRequestHandler<GetAccountBalanceQuery, AccountBalance>
{
    private readonly IWalletDbContext _walletDbContext;

    public GetAccountBalanceQueryHandler(IWalletDbContext walletDbContext)
    {
        _walletDbContext = walletDbContext;
    }

    public async Task<AccountBalance> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
    {
        var accountBalance = await _walletDbContext.AccountWallets.Where(o => o.Id == request.AccountId)
            .Select(o => new AccountBalance
            {
                AccountId = o.Id,
                AccountType = o.Account.AccountType,
                Balance = o.Account.Balance
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (accountBalance == null)
        {
            return new AccountBalance
            {
                AccountId = request.AccountId,
                AccountType = "",
                Balance = 0
            };
        }

        return accountBalance;
    }
}