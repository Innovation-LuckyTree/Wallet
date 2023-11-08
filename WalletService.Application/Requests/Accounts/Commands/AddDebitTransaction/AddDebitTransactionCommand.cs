using MediatR;

namespace WalletService.Application.Requests.Accounts.Commands.AddDebitTransaction;

public record AddDebitTransaction(Guid AccountId, string AccountType, string TransactionNo, string TransactionReference, decimal Amount, string Notes) : IRequest<Unit>;
