using System.Text.Json.Serialization;

namespace WalletService.Application.Requests.BonusAccounts.Queries.GetBonusAccountBalance;

public class BonusAccountBalance
{
    public Guid AccountId { get; set; }
    public string AccountType { get; set; }
    public decimal Balance { get; set; }
    public IEnumerable<PromotionDetail> PromotionDetails { get; set; }
}

public class PromotionDetail
{
    [JsonIgnore]
    public Guid AccountId { get; set; }
    [JsonIgnore]
    public string AccountType { get; set; }
    public long PromotionId { get; set; }
    public DateTimeOffset DateStarted { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public decimal RemainingAmount { get; set; }
    public decimal ConsumedAmount { get; set; }
}
