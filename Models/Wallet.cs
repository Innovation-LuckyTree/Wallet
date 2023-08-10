namespace Wallet.Models
{
    public record LedgerWallet
    {
        public Guid Id { get; set; }
        public Guid ReferenceID { get; set; }
        public decimal Balance { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; } = "System";
    }
}
