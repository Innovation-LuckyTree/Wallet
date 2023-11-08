using MediatR;

namespace WalletService.Application.Requests.Accounts.Commands.AddTransaction;

public record CreateAccountCommand(Guid AccountId, string AccountType, string TransactionNo, string TransactionReference, decimal Amount, string Notes) : IRequest<Unit>;
