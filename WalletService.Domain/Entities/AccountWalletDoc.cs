namespace WalletService.Domain.Entities;

public class AccountWalletDoc
{
    public Guid Id { get; set; }

    public Account Account { get; set; }
}