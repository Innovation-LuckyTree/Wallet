namespace Wallet.RequestModel;

public record TransactionRequestModel
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public Guid AccountId { get; set; }
    public string AccountType { get; set; }
    public string TransactionType { get; set; }
}
