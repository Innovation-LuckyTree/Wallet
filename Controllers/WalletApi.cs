using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using Wallet.Models;
using Wallet.Services.Interface;

namespace Wallet.Controllers
{
    public class WalletApiController : ControllerBase
    {
        ITransactionService _transactionServices;
        ILedgerService _ledgerService;
        public WalletApiController(ITransactionService transactionServices, ILedgerService ledgerService)
        {
            _transactionServices = transactionServices;
            _ledgerService = ledgerService;
        }

        [HttpPost]
        public async Task<ActionResult> SaveTransaction([FromBody] WalletLedger walletLedger)
        {
            //var result = await _transactionServices.AddAsync(transaction);
            var result = await _ledgerService.CreateNewTransactionAsync(walletLedger);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> Get(Guid referenceId)
        {
            var result = await _ledgerService.CalculateBalanceAsync(referenceId);
            return Ok(result);
        }
    }

}
