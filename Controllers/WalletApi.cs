using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using Wallet.Models;
using Wallet.RequestModel;
using Wallet.Services.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        public async Task<ActionResult> Get(Guid accountId)
        {
            var result = await _ledgerService.CalculateBalanceAsync(accountId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetByAccountId(Guid accountId)
        {
            var result = await _ledgerService.GetTransactionByAccountIdAsync(accountId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetByAccountTranscationNo(string TransactionNo)
        {
            var result = await _ledgerService.GetTransactionByTransactionNoAsync(TransactionNo);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetTransaction([FromBody] TransactionRequestModel transactionRequest)
        {
            var result = await _ledgerService.GetTransaction(transactionRequest);
            return Ok(result);
        }
    }

}
