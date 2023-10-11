
namespace Wallet.Services;

using FluentValidation;
using Wallet.Models;
public class PaymentTransactionValidator : AbstractValidator<PaymentTransaction>
{
    public PaymentTransactionValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ReferenceId).NotEmpty();
        RuleFor(x => x.TransactionType).IsInEnum();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Created).NotEmpty();
    }
}
