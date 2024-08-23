using WalletService.Domain.Enums;

namespace WalletService.Application.Requests.BonusAccounts.Queries;

public class BonusAccountTransactionDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AccountId { get; set; }
    public string TransactionNo { get; set; }
    public TransactionType TransactionType { get; set; }
    public long? PromotionId { get; set; }
    public DateTime? DateStarted { get; set; }
    public DateTime? DateExpired { get; set; }
    public string TransactionReference { get; set; }
    public decimal Amount { get; set; }
    public decimal Credit { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal PreviousBalance { get; set; } = 0;
    public string ModeOfTransaction { get; set; }
    public string Notes { get; set; }
}