namespace WalletService.Application.Requests.Accounts.Queries;

public class TransactionsAccountDto
{
    public TransactionsAccountDto(IEnumerable<AccountTransactionDto> transactions, int totalCount, decimal totalDebit, decimal totalCredit, int debitTransactionCount, int creditsTransactionCount)
    {
        Transactions = transactions;
        TotalCount = totalCount;
        TotalDebit = totalDebit;
        TotalCredit = totalCredit;
        DebitTransactionCount = debitTransactionCount;
        CreditsTransactionCount = creditsTransactionCount;
        Balance = TotalDebit - TotalCredit;
        TotalTransactionCount = DebitTransactionCount + CreditsTransactionCount;
    }

    public Guid AccountId { get; set; }
    public string AccountType { get; set; }
    public int Offset { get; set; }
    public int TotalCount { get; private set; }
    public decimal TotalDebit { get; private set; }
    public decimal TotalCredit { get; private set; }
    public int DebitTransactionCount { get; private set; }
    public int CreditsTransactionCount { get; private set; }
    public decimal TotalTransactionCount { get; private set; }

    public int TransactionCount
    {
        get
        {
            return Transactions?.Count() ?? 0;
        }
    }

    public decimal Balance { get; private set; }

    public IEnumerable<AccountTransactionDto> Transactions { get; private set; }
}