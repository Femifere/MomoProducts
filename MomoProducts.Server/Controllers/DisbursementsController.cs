using Microsoft.AspNetCore.Mvc;
using MomoProducts.Server.Interfaces.Disbursements;
using MomoProducts.Server.Interfaces.Common;
using MomoProducts.Server.Models.Disbursements;
using MomoProducts.Server.Models.Common;
using System.Threading.Tasks;

namespace MomoProducts.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisbursementsController : ControllerBase
    {
        private readonly IDepositRepository _depositRepository;
        private readonly IRefundRepository _refundRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly IAccessTokenRepository _accessTokenRepository;
        private readonly IOauth2TokenRepository _oauth2TokenRepository;

        public DisbursementsController(
            IDepositRepository depositRepository,
            IRefundRepository refundRepository,
            ITransferRepository transferRepository,
            IAccessTokenRepository accessTokenRepository,
            IOauth2TokenRepository oauth2TokenRepository)
        {
            _depositRepository = depositRepository;
            _refundRepository = refundRepository;
            _transferRepository = transferRepository;
            _accessTokenRepository = accessTokenRepository;
            _oauth2TokenRepository = oauth2TokenRepository;
        }

        // Access Token Endpoints

        [HttpPost("bc-authorize")]
        public async Task<IActionResult> BCAuthorize()
        {
            // Logic for BC Authorization
            return Ok();
        }

        [HttpPost("token")]
        public async Task<IActionResult> CreateAccessToken()
        {
            var token = await _accessTokenRepository.GetAccessTokenAsync();
            return Ok(token);
        }

        [HttpPost("oauth2/token")]
        public async Task<IActionResult> CreateOauth2Token()
        {
            var oauth2Token = await _oauth2TokenRepository.GetOauth2TokenAsync();
            return Ok(oauth2Token);
        }

        // Deposit Endpoints
        [HttpPost("v1_0/deposit")]
        public async Task<IActionResult> CreateDepositV1([FromBody] Deposit deposit)
        {
            await _depositRepository.CreateDepositAsync(deposit);
            return Ok(deposit);
        }

        [HttpPost("v2_0/deposit")]
        public async Task<IActionResult> CreateDepositV2([FromBody] Deposit deposit)
        {
            await _depositRepository.CreateDepositAsync(deposit);
            return Ok(deposit);
        }

        [HttpGet("account/balance")]
        public async Task<IActionResult> GetAccountBalance()
        {
            // Logic for getting account balance
            return Ok();
        }

        [HttpGet("account/balance/{currency}")]
        public async Task<IActionResult> GetAccountBalanceInSpecificCurrency(string currency)
        {
            // Logic for getting balance in specific currency
            return Ok();
        }

        [HttpGet("accountholder/{accountHolderIdType}/{accountHolderId}/basicuserinfo")]
        public async Task<IActionResult> GetBasicUserinfo(string accountHolderIdType, string accountHolderId)
        {
            // Logic for getting basic user info
            return Ok();
        }

        [HttpGet("deposit/{referenceId}")]
        public async Task<IActionResult> GetDepositStatus(string referenceId)
        {
            var deposit = await _depositRepository.GetDepositByReferenceIdAsync(referenceId);
            return deposit != null ? Ok(deposit) : NotFound();
        }

        // Refund Endpoints

        [HttpPost("v1_0/refund")]
        public async Task<IActionResult> CreateRefundV1([FromBody] Refund refund)
        {
            await _refundRepository.CreateRefundAsync(refund);
            return Ok(refund);
        }

        [HttpPost("v2_0/refund")]
        public async Task<IActionResult> CreateRefundV2([FromBody] Refund refund)
        {
            await _refundRepository.CreateRefundAsync(refund);
            return Ok(refund);
        }

        [HttpGet("refund/{referenceId}")]
        public async Task<IActionResult> GetRefundStatus(string referenceId)
        {
            var refund = await _refundRepository.GetRefundByReferenceIdAsync(referenceId);
            return refund != null ? Ok(refund) : NotFound();
        }

        // Transfer Endpoints

        [HttpPost("transfer")]
        public async Task<IActionResult> CreateTransfer([FromBody] Transfer transfer)
        {
            await _transferRepository.CreateTransferAsync(transfer);
            return Ok(transfer);
        }

        [HttpGet("transfer/{referenceId}")]
        public async Task<IActionResult> GetTransferStatus(string referenceId)
        {
            var transfer = await _transferRepository.GetTransferByReferenceIdAsync(referenceId);
            return transfer != null ? Ok(transfer) : NotFound();
        }

        // Additional Endpoints for User Info and Validation

        [HttpGet("oauth2/userinfo")]
        public async Task<IActionResult> GetUserInfoWithConsent()
        {
            // Logic for getting user info with consent
            return Ok();
        }

        [HttpGet("accountholder/{accountHolderIdType}/{accountHolderId}/active")]
        public async Task<IActionResult> ValidateAccountHolderStatus(string accountHolderIdType, string accountHolderId)
        {
            // Logic for validating account holder status
            return Ok();
        }
    }
}
