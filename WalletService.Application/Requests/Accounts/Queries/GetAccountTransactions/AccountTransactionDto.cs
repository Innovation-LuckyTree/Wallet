using WalletService.Domain.Enums;

namespace WalletService.Application.Requests.Accounts.Queries.GetAccountTransactions;

public class AccountTransactionDto
{
    public Guid Id { get; set; }
    public string TransactionNo { get; set; }
    public TransactionType TransactionType { get; set; }
    public string TransactionReference { get; set; }
    public decimal Amount { get; set; }
    public decimal Credit { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Notes { get; set; }
}