using WalletService.Domain.Enums;

namespace WalletService.Domain.Entities;

public class BonusWalletTransaction
{
    public long BonusWalletTransactionId { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AccountId { get; set; }
    public string TransactionNo { get; set; }
    public TransactionType TransactionType { get; set; } = TransactionType.Debit;
    public long PromotionId { get; set; }
    public DateTime DateStarted { get; set; } = DateTime.Now;
    public DateTime DateExpired { get; set; }
    public string TransactionReference { get; set; }
    public decimal Amount { get; set; }
    public decimal Credit { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.Now;
    public decimal PreviousBalance { get; set; } = 0;
    public string ModeOfTransaction { get; set; }
    public string Notes { get; set; }

    public virtual BonusAccount BonusAccount { get; set; }
}