namespace WalletService.Domain.Entities;

public class BonusAccount
{
    public BonusAccount()
    {
        BonusWalletTransactions = new HashSet<BonusWalletTransaction>();
    }

    public Guid BonusAccountId { get; set; }
    public string BonusAccountType { get; set; }
    public decimal Balance { get; set; } = 0;
    public DateTime DateUpdated { get; set; } = DateTime.Now;

    public virtual IEnumerable<BonusWalletTransaction> BonusWalletTransactions { get; set; }
}