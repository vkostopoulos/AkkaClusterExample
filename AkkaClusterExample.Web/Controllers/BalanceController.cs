using System;
using System.Net;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using static AkkaClusterExample.Protocol.BalanceProtocol;

namespace AkkaClusterExample.Web.Controllers
{
    [Route("api/balance")]
    public class BalanceController : Controller
    {
        private readonly WebService _webService;

        public class TransactionInputModel
        {
            public float Amount { get; set; }
        }

        public BalanceController(WebService webService)
        {
            _webService = webService;
        }

        /// <summary>
        /// Add Balance
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpPost("{customerId}")]
        public async Task<IActionResult> AddBalance(int customerId, [FromBody] TransactionInputModel transactionInputModel)
        {
            var result = await _webService.BalanceActor.Ask<BalanceCreationResponseMessage>(new AddBalance(customerId, transactionInputModel.Amount), TimeSpan.FromSeconds(10));
            if (result is BalanceAdded a)
            {
                return Created($"/api/balance/{customerId}", a.Balance);
            }
            else if (result is BalanceAlreadyAdded)
            {
                return Conflict("Balance for this customer already added");
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadGateway);
            }
        }

        /// <summary>
        /// Get Balance
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("GetRemainingBalance/{customerId}")]
        public async Task<IActionResult> GetRemainingBalance(int customerId)
        {
            var result = await _webService.BalanceActor.Ask<FindBalanceResponseMessage>(new FindBalance(customerId), TimeSpan.FromSeconds(60));
            if (result is FoundBalanceResponse a)
            {
                return Ok(a.Balance);
            }
            else if (result is CouldNotFindBalanceResponse)
            {
                return NotFound();
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadGateway);
            }
        }

        [HttpPost("DoDeposit/{customerId}")]
        public async Task<IActionResult> DoDeposit(int customerId, [FromBody] TransactionInputModel transactionInputModel)
        {
            var result = await _webService.BalanceActor.Ask<DoDepositResponseMessage>(new DoDeposit(customerId, transactionInputModel.Amount), TimeSpan.FromSeconds(60));
            if (result is DepositAdded a)
            {
                return Created($"/api/balance/{customerId}", a.Balance);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadGateway);
            }
        }

        [HttpPost("DoWithdraw/{customerId}")]
        public async Task<IActionResult> DoWithdraw(int customerId, [FromBody] TransactionInputModel transactionInputModel)
        {
            var result = await _webService.BalanceActor.Ask<DoWithdrawResponseMessage>(new DoWithdraw(customerId, transactionInputModel.Amount), TimeSpan.FromSeconds(60));
            if (result is WithdrawAdded a)
            {
                return Created($"/api/balance/{customerId}", a.Balance);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadGateway);
            }
        }
    }
}
