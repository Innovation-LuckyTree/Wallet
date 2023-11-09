using WalletService.Domain.Enums;

namespace WalletService.Domain.Entities;

public class WalletTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TransactionNo { get; set; }
    public TransactionType TransactionType { get; set; } = TransactionType.Debit;
    public string TransactionReference { get; set; }
    public decimal Amount { get; set; }
    public decimal Credit { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.Now;
    public string Notes { get; set; }
}