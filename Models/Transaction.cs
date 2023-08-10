namespace Wallet.Models
{
    public enum GameTransactionType { Debit, Credit };
    public record GameTransaction
    {
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        GameTransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Created { get; set; }
    }
}
