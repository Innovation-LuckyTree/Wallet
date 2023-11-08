namespace WalletService.Application.Requests.Accounts.Queries.GetAccountBalance;

public class AccountBalance
{
    public Guid AccountId { get; set; }
    public string AccountType { get; set; }
    public decimal Balance { get; set; }
}