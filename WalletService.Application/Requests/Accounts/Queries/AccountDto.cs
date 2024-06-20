using WalletService.Domain.Enums;

namespace WalletService.Application.Requests.Accounts.Queries;

public class AccountDto
{
    public AccountDto(IEnumerable<AccountTransactionDto> transactions)
    {
        Transactions = transactions;
        TotalDebit = transactions.Where(o => o.TransactionType == TransactionType.Debit).Sum(o => o.Amount);
        TotalCredit = transactions.Where(o => o.TransactionType == TransactionType.Credit).Sum(o => o.Amount) * -1;

        DebitTransactionCount = transactions.Where(o => o.TransactionType == TransactionType.Debit).Count();
        CreditsTransactionCount = transactions.Where(o => o.TransactionType == TransactionType.Credit).Count();
    }

    public Guid AccountId { get; set; }
    public string AccountType { get; set; }
    public int Offset { get; set; }
    public int TotalCount { get; set; }

    public decimal TotalDebit { get; private set; }
    public decimal TotalCredit { get; private set; }

    public int DebitTransactionCount { get; private set; }

    public int CreditsTransactionCount { get; private set; }

    public int TransactionCount
    {
        get
        {
            return Transactions?.Count() ?? 0;
        }
    }

    public decimal Balance
    {
        get
        {
            return TotalDebit - TotalCredit;
        }
    }

    public IEnumerable<AccountTransactionDto> Transactions { get; set; }
}