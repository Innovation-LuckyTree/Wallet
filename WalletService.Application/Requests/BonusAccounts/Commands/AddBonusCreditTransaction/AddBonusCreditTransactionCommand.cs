using MediatR;

namespace WalletService.Application.Requests.BonusAccounts.Commands.AddBonusCreditTransaction;

public record AddBonusCreditTransactionCommand(Guid AccountId, string AccountType, string TransactionNo,
    string TransactionReference, decimal Amount, string ModeOfTransaction, string? Notes,
    int PromotionId, DateTime DateStarted, DateTime DateExpired) : IRequest<Unit>
{
}
