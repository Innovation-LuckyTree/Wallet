namespace WalletService.Domain.Entities;

public class Account
{
    public Account()
    {
        WalletTransactions = new HashSet<WalletTransaction>();
    }

    public Guid AccountId { get; set; }
    public string AccountType { get; set; }
    public decimal Balance { get; set; } = 0;
    public DateTimeOffset DateUpdated { get; set; } = DateTime.UtcNow;

    public virtual IEnumerable<WalletTransaction> WalletTransactions { get; set; }
}