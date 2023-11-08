using MediatR;

namespace WalletService.Application.Requests.Accounts.Queries.GetAccountBalance;

public class GetAccountBalanceQuery : IRequest<AccountBalance>
{
    public Guid AccountId { get; set; }
}
