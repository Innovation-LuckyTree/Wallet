using MediatR;

namespace WalletService.Application.Requests.Accounts.Commands.AddCreditTransaction;

public record AddCreditTransactionCommand(Guid AccountId, string AccountType, string TransactionNo, string TransactionReference, decimal Amount, string ModeOfTransaction, string? Notes) : IRequest<Unit>;