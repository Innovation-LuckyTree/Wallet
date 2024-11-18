using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletService.Application.Interfaces;
using WalletService.Application.Requests.Accounts.Queries.GetAccountBalance;

namespace WalletService.Application.Requests.Accounts.Queries.GetAccountListBalance;

public class GetAccountListBalanceQuery : IRequest<AccountBalanceVm>
{
    public IEnumerable<Guid> AccountIds { get; set; }
}

public class GetAccountListBalanceQueryHandler(IWalletDbContext walletDbContext) : IRequestHandler<GetAccountListBalanceQuery, AccountBalanceVm>
{
    private readonly IWalletDbContext _walletDbContext = walletDbContext;

    public async Task<AccountBalanceVm> Handle(GetAccountListBalanceQuery request, CancellationToken cancellationToken)
    {
        var accountBalance = await _walletDbContext.Accounts.Where(o => request.AccountIds.Contains(o.AccountId))
            .Select(o => new AccountBalance
            {
                AccountId = o.AccountId,
                AccountType = o.AccountType,
                Balance = o.Balance
            })
            .ToListAsync(cancellationToken);

        return new AccountBalanceVm(accountBalance);
    }
}