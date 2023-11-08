using MediatR;

namespace WalletService.Application.Requests.Accounts.Queries.GetAccountTransactions;

public class GetAccountTransactionsQuery : IRequest<AccountDto>
{
    public Guid AccountId { get; set; }
}
