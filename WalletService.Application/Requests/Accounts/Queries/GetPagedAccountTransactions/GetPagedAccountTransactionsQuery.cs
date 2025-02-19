using MediatR;
using WalletService.Domain.Enums;

namespace WalletService.Application.Requests.Accounts.Queries.GetAccountTransactions;

public class GetPagedAccountTransactionsQuery : IRequest<TransactionsAccountDto>
{
    public Guid AccountId { get; set; }
    public string SearchKey { get; set; }
    public TransactionType? TransactionType { get; set; }
    public int Start { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public DateTime? StartDate { get; set; } = DateTime.Now.AddDays(-7);
    public DateTime? EndDate { get; set; } = DateTime.Now;
}
