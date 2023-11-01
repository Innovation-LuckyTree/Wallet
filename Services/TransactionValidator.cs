
namespace Wallet.Services;

using FluentValidation;
using Wallet.Models;
public class WalletLedgerValidator : AbstractValidator<WalletLedger>
{
    public WalletLedgerValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.AccountType).NotEmpty();
        RuleFor(x => x.TransactionNo).NotEmpty();
        RuleFor(x => x.TransactionType).NotEmpty();
        //RuleFor(x => x.TransactionType).IsInEnum();
        RuleFor(x => x.Amount).NotEmpty();
        RuleFor(x => x.TransactionDate).NotEmpty();
        RuleFor(x => x.DateCreated).NotEmpty();
    }
}
