using Microsoft.AspNetCore.Mvc;
using MomoProducts.Server.Interfaces.Disbursements;
using MomoProducts.Server.Interfaces.Common;
using MomoProducts.Server.Models.Disbursements;
using MomoProducts.Server.Models.Common;
using MomoProducts.Server.Dtos;
using System.Threading.Tasks;
using MomoProducts.Server.Services;
using System.Net.Http;
using MomoProducts.Server.Dtos.CommonDto;
using MomoProducts.Server.Dtos.DisbursementsDto;

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
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly AuthService _authService;

        public DisbursementsController(
            IDepositRepository depositRepository,
            IRefundRepository refundRepository,
            ITransferRepository transferRepository,
            IAccessTokenRepository accessTokenRepository,
            IOauth2TokenRepository oauth2TokenRepository,
            HttpClient httpClient,
            IConfiguration config,
            AuthService authService)
        {
            _depositRepository = depositRepository;
            _refundRepository = refundRepository;
            _transferRepository = transferRepository;
            _accessTokenRepository = accessTokenRepository;
            _oauth2TokenRepository = oauth2TokenRepository;
            _httpClient = httpClient;
            _config = config;
            _authService = authService;
        }

        // BC Authorize
        [HttpPost("bc-authorize")]
        public async Task<IActionResult> AuthorizeBusinessClient([FromHeader] string xTargetEnvironment, [FromHeader] string xCallbackUrl = null)
        {
            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:bc-authorize"]}";

            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);
            if (!string.IsNullOrEmpty(xCallbackUrl))
            {
                _httpClient.DefaultRequestHeaders.Add("X-Callback-Url", xCallbackUrl);
            }

            var response = await _httpClient.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Authorization failed.");
        }

        // Create Access Token
        [HttpPost("create-access-token")]
        public async Task<IActionResult> CreateAccessToken()
        {
            var encodedCredentials = await _authService.GetEncodedCredentialsAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encodedCredentials);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:CreateAccessToken"]}";
            var response = await _httpClient.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return Ok(token);
            }
            return BadRequest("Failed to create access token.");
        }

        // Create OAuth2 Token
        [HttpPost("create-oauth2-token")]
        public async Task<IActionResult> CreateOauth2Token([FromHeader] string xTargetEnvironment)
        {
            var encodedCredentials = await _authService.GetEncodedCredentialsAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encodedCredentials);
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:CreateOauth2Token"]}";
            var response = await _httpClient.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return Ok(token);
            }
            return BadRequest("Failed to create OAuth2 token.");
        }

        // Deposit Endpoints
        [HttpPost("v1_0/deposit")]
        public async Task<IActionResult> CreateDepositV1([FromBody] DepositDto depositDto, [FromHeader] string xReferenceId, [FromHeader] string xTargetEnvironment, [FromHeader] string xCallbackUrl = null)
        {
            if (depositDto == null)
                return BadRequest("Deposit details are required.");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _authService.GetLatestOauth2TokenWithBearerAsync());
            _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", xReferenceId);
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);
            if (!string.IsNullOrEmpty(xCallbackUrl))
            {
                _httpClient.DefaultRequestHeaders.Add("X-Callback-Url", xCallbackUrl);
            }

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:v1_0:deposit"]}";
            var response = await _httpClient.PostAsJsonAsync(url, depositDto);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to create deposit.");
        }

        [HttpPost("v2_0/deposit")]
        public async Task<IActionResult> CreateDepositV2([FromBody] DepositDto depositDto, [FromHeader] string xReferenceId, [FromHeader] string xTargetEnvironment, [FromHeader] string xCallbackUrl = null)
        {
            return await CreateDepositV1(depositDto, xReferenceId, xTargetEnvironment, xCallbackUrl); // Reuse the same logic
        }

        [HttpGet("account/balance")]
        public async Task<IActionResult> GetAccountBalance([FromHeader] string xTargetEnvironment)
        {
            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:account/balance"]}";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _authService.GetLatestOauth2TokenWithBearerAsync());
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to retrieve account balance.");
        }

        [HttpGet("account/balance/{currency}")]
        public async Task<IActionResult> GetAccountBalanceInSpecificCurrency(string currency, [FromHeader] string xTargetEnvironment)
        {
            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:account/balance"]}/{currency}";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _authService.GetLatestOauth2TokenWithBearerAsync());
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to retrieve account balance in specified currency.");
        }

        [HttpGet("accountholder/{accountHolderIdType}/{accountHolderId}/basicuserinfo")]
        public async Task<IActionResult> GetBasicUserinfo(string accountHolderIdType, string accountHolderId, [FromHeader] string xTargetEnvironment)
        {
            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:accountholder"]}/{accountHolderIdType}/{accountHolderId}/basicuserinfo";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _authService.GetLatestOauth2TokenWithBearerAsync());
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to retrieve basic user info.");
        }

        [HttpGet("deposit/{referenceId}")]
        public async Task<IActionResult> GetDepositStatus(string referenceId, [FromHeader] string xTargetEnvironment)
        {
            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:deposit"]}/{referenceId}";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _authService.GetLatestOauth2TokenWithBearerAsync());
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return NotFound("Deposit not found.");
        }

        // Refund Endpoints
        [HttpPost("v1_0/refund")]
        public async Task<IActionResult> CreateRefundV1([FromBody] RefundDto refundDto, [FromHeader] string xReferenceId, [FromHeader] string xTargetEnvironment, [FromHeader] string xCallbackUrl = null)
        {
            if (refundDto == null)
                return BadRequest("Refund details are required.");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _authService.GetLatestOauth2TokenWithBearerAsync());
            _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", xReferenceId);
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);
            if (!string.IsNullOrEmpty(xCallbackUrl))
            {
                _httpClient.DefaultRequestHeaders.Add("X-Callback-Url", xCallbackUrl);
            }

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:v1_0:refund"]}";
            var response = await _httpClient.PostAsJsonAsync(url, refundDto);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to create refund.");
        }

        [HttpPost("v2_0/refund")]
        public async Task<IActionResult> CreateRefundV2([FromBody] RefundDto refundDto, [FromHeader] string xReferenceId, [FromHeader] string xTargetEnvironment, [FromHeader] string xCallbackUrl = null)
        {
            return await CreateRefundV1(refundDto, xReferenceId, xTargetEnvironment, xCallbackUrl); // Reuse the same logic
        }

        [HttpGet("refund/{referenceId}")]
        public async Task<IActionResult> GetRefundStatus(string referenceId, [FromHeader] string xTargetEnvironment)
        {
            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:refund"]}/{referenceId}";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _authService.GetLatestOauth2TokenWithBearerAsync());
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return NotFound("Refund not found.");
        }

        // Transfer Endpoints
        [HttpPost("v1_0/transfer")]
        public async Task<IActionResult> CreateTransferV1([FromBody] TransferDto transferDto, [FromHeader] string xReferenceId, [FromHeader] string xTargetEnvironment, [FromHeader] string xCallbackUrl = null)
        {
            if (transferDto == null)
                return BadRequest("Transfer details are required.");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _authService.GetLatestOauth2TokenWithBearerAsync());
            _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", xReferenceId);
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);
            if (!string.IsNullOrEmpty(xCallbackUrl))
            {
                _httpClient.DefaultRequestHeaders.Add("X-Callback-Url", xCallbackUrl);
            }

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:v1_0:transfer"]}";
            var response = await _httpClient.PostAsJsonAsync(url, transferDto);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to create transfer.");
        }

        [HttpPost("v2_0/transfer")]
        public async Task<IActionResult> CreateTransferV2([FromBody] TransferDto transferDto, [FromHeader] string xReferenceId, [FromHeader] string xTargetEnvironment, [FromHeader] string xCallbackUrl = null)
        {
            return await CreateTransferV1(transferDto, xReferenceId, xTargetEnvironment, xCallbackUrl); // Reuse the same logic
        }

        [HttpGet("transfer/{referenceId}")]
        public async Task<IActionResult> GetTransferStatus(string referenceId, [FromHeader] string xTargetEnvironment)
        {
            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:transfer"]}/{referenceId}";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _authService.GetLatestOauth2TokenWithBearerAsync());
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return NotFound("Transfer not found.");
        }
        // Get User Info With Consent
        [HttpGet("userinfo")]
        public async Task<IActionResult> GetUserInfoWithConsent([FromHeader] string xTargetEnvironment)
        {
            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:GetUserInfoWithConsent"]}";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _authService.GetLatestOauth2TokenWithBearerAsync());
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return NotFound("User info not found or access denied.");
        }

        // Validate Account Holder Status
        [HttpGet("account-holder-status/{accountHolderIdType}/{accountHolderId}")]
        public async Task<IActionResult> ValidateAccountHolderStatus(string accountHolderIdType, string accountHolderId, [FromHeader] string xTargetEnvironment)
        {
            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:ValidateAccountHolderStatus"].Replace("{accountHolderIdType}", accountHolderIdType).Replace("{accountHolderId}", accountHolderId)}";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _authService.GetLatestOauth2TokenWithBearerAsync());
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return NotFound("Account holder status not found.");
        }
    }
}

