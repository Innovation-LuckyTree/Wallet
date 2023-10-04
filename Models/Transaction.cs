namespace Wallet.Models
{
    public enum PaymentTransactionType { Debit, Credit };
    public record PaymentTransaction
    {
        public Guid Id { get; init; }
        public Guid ReferenceId { get; init; }
        public PaymentTransactionType TransactionType { get; init; }
        public decimal Amount { get; init; }
        public DateTime? Created { get; init; }
    }
}
