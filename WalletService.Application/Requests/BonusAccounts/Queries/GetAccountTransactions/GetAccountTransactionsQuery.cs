using MediatR;

namespace WalletService.Application.Requests.BonusAccounts.Queries.GetAccountTransactions;

public class GetAccountTransactionsQuery : IRequest<BonusAccountDto>
{
    public Guid AccountId { get; set; }
}
