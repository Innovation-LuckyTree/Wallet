using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Requests.BonusAccounts.Commands.AddBonusCreditTransaction;
using WalletService.Application.Requests.BonusAccounts.Commands.AddBonusDebitTransaction;
using WalletService.Application.Requests.BonusAccounts.Queries.GetAccountTransactions;
using WalletService.Application.Requests.BonusAccounts.Queries.GetBonusAccountBalance;
using WalletService.Application.Requests.BonusAccounts.Queries.GetBonusPromotionBalance;
using WalletService.Application.Requests.BonusAccounts.Queries.GetPagedBonusTransactions;

namespace WalletService.API.Controllers;

[Route("api/bonus-account")]
public class BonusAccountController : AuthorizedApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAccountTransactionsQuery { AccountId = id }, cancellationToken);
        return Ok(result);
    }

    [HttpGet("balance/{id}")]
    public async Task<IActionResult> GetAccountBalance(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetBonusAccountBalanceQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("transaction/search")]
    public async Task<IActionResult> SearchBonusTransactions([FromBody] GetPagedBonusTransactionsQuery request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost("transaction/promotion")]
    public async Task<IActionResult> GetPromoTransactions([FromBody] GetBonusPromotionBalanceQuery request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost("credit")]
    public async Task<IActionResult> Post(AddBonusCreditTransactionCommand request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return Ok();
    }


    [HttpPost("debit")]
    public async Task<IActionResult> AddDebitTransaction(AddBonusDebitTransactionCommand request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return Ok();
    }
}
