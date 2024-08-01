using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Requests.Accounts.Commands.AddCreditTransaction;
using WalletService.Application.Requests.Accounts.Commands.AddDebitTransaction;
using WalletService.Application.Requests.Accounts.Queries.GetAccountBalance;
using WalletService.Application.Requests.Accounts.Queries.GetAccountTransactions;

namespace WalletService.API.Controllers;

[Route("api/account")]
public class AccountController : AuthorizedApiControllerBase
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
        var result = await Mediator.Send(new GetAccountBalanceQuery { AccountId = id }, cancellationToken);
        return Ok(result);
    }

    [HttpPost("transaction/search")]
    public async Task<IActionResult> SearchTransactions([FromBody] GetPagedAccountTransactionsQuery request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost("credit")]
    public async Task<IActionResult> Post(AddCreditTransactionCommand request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return Ok();
    }

    [HttpPost("debit")]
    public async Task<IActionResult> AddDebitTransaction(AddDebitTransaction request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return Ok();
    }
}
