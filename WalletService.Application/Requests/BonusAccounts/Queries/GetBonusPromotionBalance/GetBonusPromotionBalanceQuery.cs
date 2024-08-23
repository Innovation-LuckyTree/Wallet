using MediatR;

namespace WalletService.Application.Requests.BonusAccounts.Queries.GetBonusPromotionBalance;

public record GetBonusPromotionBalanceQuery(Guid AccountId, long PromotionId, DateTime DateStart, DateTime DateExpired) : IRequest<BonusAccountVm>;
