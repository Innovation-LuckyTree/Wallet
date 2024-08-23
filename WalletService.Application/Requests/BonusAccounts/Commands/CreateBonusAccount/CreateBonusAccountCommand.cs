using MediatR;

namespace WalletService.Application.Requests.BonusAccounts.Commands.CreateBonusAccount;

public record CreateBonusAccountCommand(Guid AccountId, string AccountType, string TransactionNo, string TransactionReference,
    decimal Amount, string ModeOfTransaction, string Notes,
    long PromotionId, DateTime DateStarted, DateTime DateExpired) : IRequest<Unit>;

