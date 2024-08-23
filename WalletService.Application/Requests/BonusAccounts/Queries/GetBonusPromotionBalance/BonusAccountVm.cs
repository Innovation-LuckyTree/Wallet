namespace WalletService.Application.Requests.BonusAccounts.Queries.GetBonusPromotionBalance;

public class BonusAccountVm
{
    public Guid AccountId { get; set; }
    public long PromotionId { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateExpired { get; set; }
    public decimal ConsumedAmount { get; set; }
    public decimal Balance { get; set; }
    
    public IEnumerable<BonusAccountTransactionDto> AccountTransactions { get; set; }
}