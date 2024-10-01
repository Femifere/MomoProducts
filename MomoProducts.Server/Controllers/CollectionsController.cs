using Microsoft.AspNetCore.Mvc;
using MomoProducts.Server.Interfaces.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MomoProducts.Server.Models.Collections;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System;
using MomoProducts.Server.Interfaces.Common;
using MomoProducts.Server.Models.Common;
using MomoProducts.Server.Repositories.Common;

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

        public CollectionsController(
            IInvoiceRepository invoiceRepository,
            IPaymentRepository paymentRepository,
            IPreApprovalRepository preApprovalRepository,
            IRequestToPayRepository requestToPayRepository,
            IRequestToWithdrawRepository requestToWithdrawRepository,
            IAccessTokenRepository accessTokenRepository,
            IOauth2TokenRepository oauth2TokenRepository,
            HttpClient httpClient,
            IConfiguration config)
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
        }

        #region Helper Methods

        private async Task<string> GetAccessTokenAsync()
        {
            var token = await _accessTokenRepository.GetAccessTokenAsync();
            if (token == null || token.ExpiresIn < DateTime.UtcNow)
            {
                // Request a new access token
                var authUrl = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:CreateAccessToken"]}";

                var clientId = _config["MomoApi:ClientId"];
                var clientSecret = _config["MomoApi:ClientSecret"];
                var body = new Dictionary<string, string>
                {
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "grant_type", "client_credentials" }
                };

                var requestContent = new FormUrlEncodedContent(body);
                var response = await _httpClient.PostAsync(authUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(responseContent);

                    var newToken = new AccessToken
                    {
                        accessToken = tokenResponse.access_token,
                        ExpiresIn = DateTime.UtcNow.AddSeconds(tokenResponse.expires_in - 60) // Subtract 60 seconds as buffer
                    };

                    // Save the new token
                    await _accessTokenRepository.SaveAccessTokenAsync(newToken);

                    return newToken.accessToken;
                }
                else
                {
                    throw new Exception("Failed to obtain access token from Momo API.");
                }
            }

            return token.accessToken;
        }

        private string ReplaceUrlPlaceholders(string url, Dictionary<string, string> placeholders)
        {
            foreach (var placeholder in placeholders)
            {
                url = url.Replace($"{{{placeholder.Key}}}", placeholder.Value);
            }
            return url;
        }

        #endregion

        #region Invoice Endpoints

        [HttpPost("create-invoice")]
        public async Task<IActionResult> CreateInvoice([FromBody] Invoice invoice)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:CreateInvoice"]}";

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.PostAsJsonAsync(url, invoice);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var createdInvoice = JsonConvert.DeserializeObject<Invoice>(responseContent);

                    await _invoiceRepository.CreateInvoiceAsync(createdInvoice);
                    return Ok(createdInvoice);
                }

                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                // Log exception (not implemented here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("invoice-status/{referenceId}")]
        public async Task<IActionResult> GetInvoiceStatus(string referenceId)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var endpoint = _config["MomoApi:Collections:GetInvoiceStatus"];
                var url = ReplaceUrlPlaceholders($"{_config["MomoApi:BaseUrl"]}{endpoint}", new Dictionary<string, string>
                {
                    { "x-referenceId", referenceId }
                });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var status = await response.Content.ReadAsStringAsync();
                    return Ok(status);
                }

                return NotFound("Invoice status not found.");
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("cancel-invoice/{referenceId}")]
        public async Task<IActionResult> CancelInvoice(string referenceId)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var endpoint = _config["MomoApi:Collections:CancelInvoice"];
                var url = ReplaceUrlPlaceholders($"{_config["MomoApi:BaseUrl"]}{endpoint}", new Dictionary<string, string>
                {
                    { "referenceId", referenceId }
                });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Optionally update the invoice status in the repository
                    var invoice = await _invoiceRepository.GetInvoiceByReferenceIdAsync(referenceId);
                    if (invoice != null)
                    {
                        invoice.Status = "Cancelled";
                        await _invoiceRepository.UpdateInvoiceAsync(invoice);
                    }

                    return Ok("Invoice cancelled successfully.");
                }

                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region Payment Endpoints

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:CreatePayments"]}";

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.PostAsJsonAsync(url, payment);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var createdPayment = JsonConvert.DeserializeObject<Payment>(responseContent);

                    await _paymentRepository.CreatePaymentAsync(createdPayment);
                    return Ok(createdPayment);
                }

                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("payment-status/{referenceId}")]
        public async Task<IActionResult> GetPaymentStatus(string referenceId)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var endpoint = _config["MomoApi:Collections:GetPaymentStatus"];
                var url = ReplaceUrlPlaceholders($"{_config["MomoApi:BaseUrl"]}{endpoint}", new Dictionary<string, string>
                {
                    { "x-referenceId", referenceId }
                });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var status = await response.Content.ReadAsStringAsync();
                    return Ok(status);
                }

                return NotFound("Payment status not found.");
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region PreApproval Endpoints

        [HttpPost("preapproval")]
        public async Task<IActionResult> CreatePreApproval([FromBody] PreApproval preApproval)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:PreApproval"]}";

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.PostAsJsonAsync(url, preApproval);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var createdPreApproval = JsonConvert.DeserializeObject<PreApproval>(responseContent);

                    await _preApprovalRepository.CreatePreApprovalAsync(createdPreApproval);
                    return Ok(createdPreApproval);
                }

                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("preapproval-status/{referenceId}")]
        public async Task<IActionResult> GetPreApprovalStatus(string referenceId)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var endpoint = _config["MomoApi:Collections:GetPreApprovalStatus"];
                var url = ReplaceUrlPlaceholders($"{_config["MomoApi:BaseUrl"]}{endpoint}", new Dictionary<string, string>
                {
                    { "referenceId", referenceId }
                });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var status = await response.Content.ReadAsStringAsync();
                    return Ok(status);
                }

                return NotFound("Pre-approval status not found.");
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("cancel-preapproval/{preapprovalId}")]
        public async Task<IActionResult> CancelPreApproval(string preapprovalId)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var endpoint = _config["MomoApi:Collections:CancelPreApproval"];
                var url = ReplaceUrlPlaceholders($"{_config["MomoApi:BaseUrl"]}{endpoint}", new Dictionary<string, string>
                {
                    { "preapprovalid", preapprovalId }
                });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Optionally update the pre-approval status in the repository
                    var preApproval = await _preApprovalRepository.GetPreApprovalByReferenceIdAsync(preapprovalId);
                    if (preApproval != null)
                    {
                        preApproval.Status = "Cancelled";
                        await _preApprovalRepository.UpdatePreApprovalAsync(preApproval);
                    }

                    return Ok("Pre-approval cancelled successfully.");
                }

                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region Request to Pay Endpoints

        [HttpPost("request-to-pay")]
        public async Task<IActionResult> RequestToPay([FromBody] RequesttoPay requestToPay)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:RequesttoPay"]}";

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.PostAsJsonAsync(url, requestToPay);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var createdRequest = JsonConvert.DeserializeObject<RequesttoPay>(responseContent);

                    await _requestToPayRepository.CreateRequestToPayAsync(createdRequest);
                    return Ok(createdRequest);
                }

                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("request-to-pay-status/{referenceId}")]
        public async Task<IActionResult> GetRequestToPayStatus(string referenceId)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var endpoint = _config["MomoApi:Collections:RequesttoPayTransactionStatus"];
                var url = ReplaceUrlPlaceholders($"{_config["MomoApi:BaseUrl"]}{endpoint}", new Dictionary<string, string>
                {
                    { "referenceId", referenceId }
                });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var status = await response.Content.ReadAsStringAsync();
                    return Ok(status);
                }

                return NotFound("Request to Pay status not found.");
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("request-to-pay-delivery-notification/{referenceId}")]
        public async Task<IActionResult> RequestToPayDeliveryNotification(string referenceId, [FromBody] DeliveryNotification notification)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var endpoint = _config["MomoApi:Collections:RequesttoPayDeliveryNotification"];
                var url = ReplaceUrlPlaceholders($"{_config["MomoApi:BaseUrl"]}{endpoint}", new Dictionary<string, string>
                {
                    { "referenceId", referenceId }
                });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.PostAsJsonAsync(url, notification);

                if (response.IsSuccessStatusCode)
                {
                    return Ok("Delivery notification sent successfully.");
                }

                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region Request to Withdraw Endpoints

        [HttpPost("request-to-withdraw-v1")]
        public async Task<IActionResult> RequestToWithdrawV1([FromBody] RequestToWithdraw requestToWithdraw)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:RequestToWithdraw-V1"]}";

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.PostAsJsonAsync(url, requestToWithdraw);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var createdRequest = JsonConvert.DeserializeObject<RequestToWithdraw>(responseContent);

                    await _requestToWithdrawRepository.CreateRequestToWithdrawAsync(createdRequest);
                    return Ok(createdRequest);
                }

                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("request-to-withdraw-v2")]
        public async Task<IActionResult> RequestToWithdrawV2([FromBody] RequestToWithdraw requestToWithdraw)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:RequestToWithdraw-V2"]}";

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.PostAsJsonAsync(url, requestToWithdraw);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var createdRequest = JsonConvert.DeserializeObject<RequestToWithdraw>(responseContent);

                    await _requestToWithdrawRepository.CreateRequestToWithdrawAsync(createdRequest);
                    return Ok(createdRequest);
                }

                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("request-to-withdraw-status/{referenceId}")]
        public async Task<IActionResult> GetRequestToWithdrawStatus(string referenceId)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var endpoint = _config["MomoApi:Collections:RequestToWithdrawTransactionStatus"];
                var url = ReplaceUrlPlaceholders($"{_config["MomoApi:BaseUrl"]}{endpoint}", new Dictionary<string, string>
                {
                    { "referenceId", referenceId }
                });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var status = await response.Content.ReadAsStringAsync();
                    return Ok(status);
                }

                return NotFound("Request to Withdraw status not found.");
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region Account Balance Endpoints

        [HttpGet("account-balance")]
        public async Task<IActionResult> GetAccountBalance()
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:GetAccountBalance"]}";

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var balance = await response.Content.ReadAsStringAsync();
                    return Ok(balance);
                }

                return BadRequest("Failed to retrieve account balance.");
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("account-balance/{currency}")]
        public async Task<IActionResult> GetAccountBalanceInSpecificCurrency(string currency)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var endpoint = _config["MomoApi:Collections:GetAccountBalancelnSpecificCurrency"];
                var url = ReplaceUrlPlaceholders($"{_config["MomoApi:BaseUrl"]}{endpoint}", new Dictionary<string, string>
                {
                    { "currency", currency }
                });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var balance = await response.Content.ReadAsStringAsync();
                    return Ok(balance);
                }

                return BadRequest("Failed to retrieve account balance for the specified currency.");
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region User Info Endpoints

        [HttpGet("basic-userinfo/{accountHolderIdType}/{accountHolderId}")]
        public async Task<IActionResult> GetBasicUserInfo(string accountHolderIdType, string accountHolderId)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var endpoint = _config["MomoApi:Collections:GetBasicUserinfo"];
                var url = ReplaceUrlPlaceholders($"{_config["MomoApi:BaseUrl"]}{endpoint}", new Dictionary<string, string>
                {
                    { "accountHolderIdType", accountHolderIdType },
                    { "accountHolderId", accountHolderId }
                });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var userInfo = await response.Content.ReadAsStringAsync();
                    return Ok(userInfo);
                }

                return NotFound("User info not found.");
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user-info-with-consent")]
        public async Task<IActionResult> GetUserInfoWithConsent()
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:GetUserInfoWithConsent"]}";

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var userInfo = await response.Content.ReadAsStringAsync();
                    return Ok(userInfo);
                }

                return BadRequest("Failed to retrieve user info with consent.");
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region PreApproval User Endpoints

        [HttpGet("approved-preapprovals/{accountHolderIdType}/{accountHolderId}")]
        public async Task<IActionResult> GetApprovedPreApprovals(string accountHolderIdType, string accountHolderId)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var endpoint = _config["MomoApi:Collections:GetApprovedPreApprovals"];
                var url = ReplaceUrlPlaceholders($"{_config["MomoApi:BaseUrl"]}{endpoint}", new Dictionary<string, string>
                {
                    { "accountHolderIdType", accountHolderIdType },
                    { "accountHolderId", accountHolderId }
                });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var preApprovals = await response.Content.ReadAsStringAsync();
                    return Ok(preApprovals);
                }

                return BadRequest("Failed to retrieve approved pre-approvals.");
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region Account Holder Validation

        [HttpGet("validate-account-holder/{accountHolderIdType}/{accountHolderId}")]
        public async Task<IActionResult> ValidateAccountHolderStatus(string accountHolderIdType, string accountHolderId)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var endpoint = _config["MomoApi:Collections:ValidateAccountHolderStatus"];
                var url = ReplaceUrlPlaceholders($"{_config["MomoApi:BaseUrl"]}{endpoint}", new Dictionary<string, string>
                {
                    { "accountHolderIdType", accountHolderIdType },
                    { "accountHolderId", accountHolderId }
                });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var status = await response.Content.ReadAsStringAsync();
                    return Ok(status);
                }

                return BadRequest("Failed to validate account holder status.");
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region OAuth2 Token Endpoints

        [HttpPost("create-oauth2-token")]
        public async Task<IActionResult> CreateOauth2Token([FromBody] Oauth2TokenRequest tokenRequest)
        {
            try
            {
                var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:CreateOauth2Token"]}";

                var clientId = _config["MomoApi:ClientId"];
                var clientSecret = _config["MomoApi:ClientSecret"];
                var body = new Dictionary<string, string>
                {
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "grant_type", "client_credentials" }
                };

                var requestContent = new FormUrlEncodedContent(body);
                var response = await _httpClient.PostAsync(url, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<Oauth2TokenResponse>(responseContent);

                    var newToken = new Oauth2Token
                    {
                        AccessToken = tokenResponse.access_token,
                        ExpiresIn = DateTime.UtcNow.AddSeconds(tokenResponse.expires_in - 60) // Subtract 60 seconds as buffer
                    };

                    // Save the new token
                    await _oauth2TokenRepository.SaveOauth2TokenAsync(newToken);

                    return Ok(newToken);
                }

                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        // Implement other endpoints similarly following the above patterns
    }

    #region Supporting Classes

    // These classes represent the structure of the token responses.
    // Adjust the properties based on the actual response from Momo API.

    public class AccessTokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
    }

    public class Oauth2TokenRequest
    {
        // Define properties as per Momo API requirements
    }

    public class Oauth2TokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
    }

    public class DeliveryNotification
    {
        // Define properties based on Momo API requirements
    }

    #endregion
}
