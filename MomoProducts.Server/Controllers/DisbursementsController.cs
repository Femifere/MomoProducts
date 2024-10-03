namespace MomoProducts.Server.Controllers
{
    using MomoProducts.Server.Interfaces.Disbursements;
    using MomoProducts.Server.Models.Common;
    using MomoProducts.Server.Models.Disbursements;
    using MomoProducts.Server.Services;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using MomoProducts.Server.Interfaces.Common;

    [Route("api/[controller]")]
    [ApiController]
    public class DisbursementsController : ControllerBase
    {
        private readonly IDepositRepository _depositRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly AuthService _authService;
        private readonly string _subscriptionKey;

        public DisbursementsController(
            IDepositRepository depositRepository,
            ITransferRepository transferRepository,
            IConfiguration config,
            ILogger<DisbursementsController> logger,
            AuthService authService)
        {
            _depositRepository = depositRepository;
            _transferRepository = transferRepository;
            _config = config;
            _logger = logger;
            _authService = authService;
            _subscriptionKey = _config["MomoApi:SubscriptionKey:dummyDisbursements"];
        }

        // DTO for Access Token Response
        public class AccessTokenResponse2
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

        // Create Access Token
        [HttpPost("create-access-token")]
        public async Task<AccessTokenResponse2> CreateAccessToken()
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
                    return JsonConvert.DeserializeObject<AccessTokenResponse2>(tokenJson);
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

        // Create Deposit
        [HttpPost("create-deposit")]
        public async Task<IActionResult> CreateDeposit([FromBody] Deposit request)
        {
            try
            {
                // Check for required fields and assign default values
                if (string.IsNullOrEmpty(request.Amount) || string.IsNullOrEmpty(request.Currency) ||
                    string.IsNullOrEmpty(request.ExternalId) || request.Payee == null || string.IsNullOrEmpty(request.Payee.PartyId))
                {
                    return BadRequest(new { Success = false, Message = "Deposit details are incomplete." });
                }

                var accessTokenResponse = await CreateAccessToken();
                var accessToken = accessTokenResponse.access_token;

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("X-Reference-Id", Guid.NewGuid().ToString());
                client.DefaultRequestHeaders.Add("X-Target-Environment", "sandbox");
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };

                var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Disbursements:Deposit-V2"]}";

                var jsonContent = JsonConvert.SerializeObject(new
                {
                    amount = request.Amount,
                    currency = request.Currency,
                    externalId = request.ExternalId,
                    payee = new
                    {
                        partyIdType = request.Payee.PartyIdType,
                        partyId = request.Payee.PartyId
                    },
                    payerMessage = request.PayerMessage,
                    payeeNote = request.PayeeNote
                });

                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    await _depositRepository.CreateDepositAsync(request); // Store deposit in database
                    return Ok(new { Success = true, Data = result });
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return BadRequest(new { Success = false, Message = $"Failed to create deposit. Response: {errorContent}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = $"An error occurred: {ex.Message}" });
            }
        }


        // Get Deposit Status
        [HttpGet("get-deposit-status/{referenceId}")]
        public async Task<IActionResult> GetDepositStatus(string referenceId)
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

                var uri = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Deposits:GetDepositStatus"].Replace("{referenceId}", referenceId)}";
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Ok(new { Success = true, Data = result });
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return BadRequest(new { Success = false, Message = $"Failed to retrieve deposit status. Response: {errorContent}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = $"An error occurred: {ex.Message}" });
            }
        }

    }
}
