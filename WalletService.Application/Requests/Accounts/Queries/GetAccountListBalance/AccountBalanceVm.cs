using WalletService.Application.Requests.Accounts.Queries.GetAccountBalance;

namespace WalletService.Application.Requests.Accounts.Queries.GetAccountListBalance;

public record AccountBalanceVm(IEnumerable<AccountBalance> AccountBalances)
{
    public int Count { get; } = AccountBalances?.Count() ?? 0;
}