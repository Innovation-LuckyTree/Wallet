namespace Wallet.Models
{
    public record WalletLedger
    {
        public Guid Id { get; init; }
        public Guid AccountId { get; init; }
        public string AccountType { get; init; }
        public string TransactionNo { get; init; }
        public string TransactionType { get; init; }
        public decimal Amount { get; init; }
        public DateTime? TransactionDate { get; init; }
        public DateTime? DateCreated { get; init; }
    }
}
