using Microsoft.AspNetCore.Mvc;
using MomoProducts.Server.Interfaces.Collections;
using MomoProducts.Server.Models.Collections;
using MomoProducts.Server.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MomoProducts.Server.Interfaces.Common;
using MomoProducts.Server.Dtos.CollectionsDto;

namespace MomoProducts.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionsController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPreApprovalRepository _preApprovalRepository;
        private readonly IRequestToPayRepository _requestToPayRepository;
        private readonly IRequestToWithdrawRepository _requestToWithdrawRepository;
        private readonly IAccessTokenRepository _accessTokenRepository;
        private readonly IOauth2TokenRepository _oauth2TokenRepository;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly AuthService _authService;

        public CollectionsController(
            IInvoiceRepository invoiceRepository,
            IPaymentRepository paymentRepository,
            IPreApprovalRepository preApprovalRepository,
            IRequestToPayRepository requestToPayRepository,
            IRequestToWithdrawRepository requestToWithdrawRepository,
            IAccessTokenRepository accessTokenRepository,
            IOauth2TokenRepository oauth2TokenRepository,
            HttpClient httpClient,
            IConfiguration config,
            AuthService authService)
        {
            _invoiceRepository = invoiceRepository;
            _paymentRepository = paymentRepository;
            _preApprovalRepository = preApprovalRepository;
            _requestToPayRepository = requestToPayRepository;
            _requestToWithdrawRepository = requestToWithdrawRepository;
            _accessTokenRepository = accessTokenRepository;
            _oauth2TokenRepository = oauth2TokenRepository;
            _httpClient = httpClient;
            _config = config;
            _authService = authService;
        }

        #region Authorization & Token Endpoints

        // Create Access Token
        [HttpPost("create-access-token")]
        public async Task<IActionResult> CreateAccessToken()
        {
            var encodedCredentials = await _authService.GetEncodedCredentialsAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:CreateAccessToken"]}";
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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
            _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", xTargetEnvironment);
            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:CreateOauth2Token"]}";
            var response = await _httpClient.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return Ok(token);
            }
            return BadRequest("Failed to create OAuth2 token.");
        }

        // BC Authorize
        [HttpPost("bc-authorize")]
        public async Task<IActionResult> AuthorizeBusinessClient([FromHeader] string xTargetEnvironment, [FromHeader] string xCallbackUrl = null)
        {
            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:bc-authorize"]}";
            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
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

        #endregion

        #region Invoice Endpoints

        // Create Invoice
        [HttpPost("create-invoice")]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceDto invoiceDto)
        {
            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:CreateInvoice"]}";
            var content = new StringContent(JsonConvert.SerializeObject(invoiceDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to create invoice.");
        }

        // Cancel Invoice
        [HttpDelete("cancel-invoice/{referenceId}")]
        public async Task<IActionResult> CancelInvoice(string referenceId)
        {
            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:CancelInvoice"].Replace("{referenceId}", referenceId)}";
            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok("Invoice cancelled.");
            }
            return BadRequest("Failed to cancel invoice.");
        }

        #endregion


        #region Payment Endpoints

        // Create Payment
        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto paymentDto)
        {
            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:CreatePayments"]}";
            var content = new StringContent(JsonConvert.SerializeObject(paymentDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to create payment.");
        }

        // Get Payment Status
        [HttpGet("payment-status/{referenceId}")]
        public async Task<IActionResult> GetPaymentStatus(string referenceId)
        {
            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:GetPaymentStatus"].Replace("{x-referenceId}", referenceId)}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to get payment status.");
        }

        #endregion

        #region Pre-Approval Endpoints

        // Create PreApproval
        [HttpPost("create-preapproval")]
        public async Task<IActionResult> CreatePreApproval([FromBody] PreApprovalDto preApprovalDto)
        {
            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:PreApproval"]}";
            var content = new StringContent(JsonConvert.SerializeObject(preApprovalDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to create pre-approval.");
        }

        // Get PreApproval Status
        [HttpGet("preapproval-status/{referenceId}")]
        public async Task<IActionResult> GetPreApprovalStatus(string referenceId)
        {
            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:GetPreApprovalStatus"].Replace("{referenceId}", referenceId)}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to get pre-approval status.");
        }

        // Cancel PreApproval
        [HttpDelete("cancel-preapproval/{preapprovalid}")]
        public async Task<IActionResult> CancelPreApproval(string preapprovalid)
        {
            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:CancelPreApproval"].Replace("{preapprovalid}", preapprovalid)}";
            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok("Pre-approval cancelled.");
            }
            return BadRequest("Failed to cancel pre-approval.");
        }

        #endregion

        #region Request to Pay Endpoints

        // Request to Pay
        [HttpPost("request-to-pay")]
        public async Task<IActionResult> RequestToPay([FromBody] RequestToPayDto requestToPayDto)
        {
            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:RequestToPay"]}";
            var content = new StringContent(JsonConvert.SerializeObject(requestToPayDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to send request to pay.");
        }

        // Get Request to Pay Status
        [HttpGet("request-to-pay-status/{referenceId}")]
        public async Task<IActionResult> GetRequestToPayStatus(string referenceId)
        {
            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:GetRequestToPayStatus"].Replace("{x-referenceId}", referenceId)}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to get request to pay status.");
        }

        

        #endregion

        #region Request to Withdraw Endpoints

        // Request to Withdraw
        [HttpPost("request-to-withdraw")]
        public async Task<IActionResult> RequestToWithdraw([FromBody] RequestToWithdrawDto requestToWithdrawDto)
        {
            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:RequestToWithdraw"]}";
            var content = new StringContent(JsonConvert.SerializeObject(requestToWithdrawDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to send request to withdraw.");
        }

        // Get Request to Withdraw Status
        [HttpGet("request-to-withdraw-status/{referenceId}")]
        public async Task<IActionResult> GetRequestToWithdrawStatus(string referenceId)
        {
            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:GetRequestToWithdrawStatus"].Replace("{x-referenceId}", referenceId)}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to get request to withdraw status.");
        }

        #endregion

        #region Account Holder Validation Endpoints

        // Validate Account Holder
        [HttpGet("validate-account-holder-status/{accountHolderId}")]
        public async Task<IActionResult> ValidateAccountHolderStatus(string accountHolderId)
        {
            var bearerToken = await _authService.GetLatestOauth2TokenWithBearerAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:ValidateAccountHolderStatus"].Replace("{accountHolderId}", accountHolderId)}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            return BadRequest("Failed to validate account holder.");
        }

        #endregion
    }
}
