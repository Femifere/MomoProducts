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
        private readonly string _subscriptionKey; 
        private readonly ILogger _logger;

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
            AuthService authService,
            ILogger<DisbursementsController> logger
            )
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
            _logger = logger;
            _subscriptionKey = _config["MomoApi:SubscriptionKey:dummyCollections"];
        }
        // DTO for Access Token Response
        public class AccessTokenResponse3
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

        // Create Access Token
        [HttpPost("create-access-token")]
        public async Task<AccessTokenResponse3> CreateAccessToken()
        {
            try
            {
                var encodedCredentials = await _authService.GetEncodedCredentialsAsync();
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config["MomoApi:SubscriptionKey:dummyRemittance"]);

                var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:CreateAccessToken"]}";
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    var tokenJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AccessTokenResponse3>(tokenJson);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create access token. Response: {errorContent}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        

        // DTO for RequestToPay Response
        public class RequestToPayResponse
        {
            public string resourceId { get; set; }
            public string status { get; set; }
           
        }

        // Create Request to Pay
        [HttpPost("create-request-to-pay")]
        public async Task<IActionResult> CreateRequestToPay([FromBody] RequesttoPay request)
        {
            try
            {
                // Check for required fields and assign default values
                if (string.IsNullOrEmpty(request.Amount) || string.IsNullOrEmpty(request.Currency) ||
                    string.IsNullOrEmpty(request.ExternalId) || request.Payer == null || string.IsNullOrEmpty(request.Payer.PartyId))
                {
                    return BadRequest(new { Success = false, Message = "Request to pay details are incomplete." });
                }

                var accessTokenResponse = await CreateAccessToken();
                var accessToken = accessTokenResponse.access_token;

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("X-Reference-Id", Guid.NewGuid().ToString());
                client.DefaultRequestHeaders.Add("X-Target-Environment", "sandbox");
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };

                var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collection:RequestToPay"]}";

                var jsonContent = JsonConvert.SerializeObject(new
                {
                    amount = request.Amount,
                    currency = request.Currency,
                    externalId = request.ExternalId,
                    payer = new
                    {
                        partyIdType = request.Payer.PartyIdType,
                        partyId = request.Payer.PartyId
                    },
                    payerMessage = request.PayerMessage,
                    payeeNote = request.PayeeNote
                });

                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Ok(new { Success = true, Data = result });
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return BadRequest(new { Success = false, Message = $"Failed to create request to pay. Response: {errorContent}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = $"An error occurred: {ex.Message}" });
            }
        }

        // Get Request to Pay Status
        [HttpGet("get-request-to-pay-status/{resourceId}")]
        public async Task<IActionResult> GetRequestToPayStatus(string resourceId)
        {
            try
            {
                var accessTokenResponse = await CreateAccessToken();
                var accessToken = accessTokenResponse.access_token;

                using var client = new HttpClient();

                // Set request headers
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("X-Target-Environment", "sandbox");
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

                var uri = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Collections:RequesttoPayTransactionStatus"].Replace("{resourceId}", resourceId)}";
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Ok(new { Success = true, Data = result });
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return BadRequest(new { Success = false, Message = $"Failed to retrieve request to pay status. Response: {errorContent}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = $"An error occurred: {ex.Message}" });
            }
        }

        

      
    }
}
