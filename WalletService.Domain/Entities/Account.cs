namespace WalletService.Domain.Entities;

public class Account
{
    public Account()
    {
        WalletTransactions = new List<WalletTransaction>();
    }

    public Guid AccountId { get; set; }
    public string AccountType { get; set; }
    public decimal Balance { get; set; } = 0;
    public DateTime DateUpdated { get; set; } = DateTime.Now;

    public IList<WalletTransaction> WalletTransactions { get; set; }
}