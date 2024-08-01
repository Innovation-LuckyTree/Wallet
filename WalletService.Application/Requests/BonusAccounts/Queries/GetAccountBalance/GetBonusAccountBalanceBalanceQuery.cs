using MediatR;

namespace WalletService.Application.Requests.BonusAccounts.Queries.GetBonusAccountBalance;

public record GetBonusAccountBalanceQuery(Guid AccountId) : IRequest<BonusAccountBalance>;
