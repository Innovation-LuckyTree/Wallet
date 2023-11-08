using WalletService.Domain.Enums;

namespace WalletService.Application.Requests.Accounts.Queries.GetAccountTransactions;

public class AccountDto
{
    public AccountDto(IEnumerable<AccountTransactionDto> transactions)
    {
        Transactions = transactions;
        TotalDebit = transactions.Where(o => o.TransactionType == TransactionType.Debit).Sum(o => o.Amount);
        TotalCredit = transactions.Where(o => o.TransactionType == TransactionType.Credit).Sum(o => o.Amount) * -1;
    }

    public Guid AccountId { get; set; }
    public string AccountType { get; set; }

    public decimal TotalDebit { get; private set; }
    public decimal TotalCredit { get; private set; }
    public decimal Balance
    {
        get
        {
            return TotalDebit - TotalCredit;
        }
    }

    public IEnumerable<AccountTransactionDto> Transactions { get; set; }
}