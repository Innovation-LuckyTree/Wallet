using MediatR;

namespace WalletService.Application.Requests.BonusAccounts.Commands.AddBonusDebitTransaction;

public record AddBonusDebitTransactionCommand(Guid AccountId, string AccountType, string TransactionNo,
    string TransactionReference, string ModeOfTransaction, decimal Amount, string Notes,
    long PromotionId, DateTime DateStarted, DateTime DateExpired) : IRequest<Unit>;
