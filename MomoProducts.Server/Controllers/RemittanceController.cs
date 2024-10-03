using Microsoft.AspNetCore.Mvc;
using MomoProducts.Server.Dtos;
using MomoProducts.Server.Interfaces.Common;
using MomoProducts.Server.Interfaces.Remittance;
using MomoProducts.Server.Models.Common;
using MomoProducts.Server.Services;
using Newtonsoft.Json; // Add this for JSON deserialization
using System.Net.Http.Headers;

[Route("api/[controller]")]
[ApiController]
public class RemittanceController : ControllerBase
{
    private readonly ICashTransferRepository _cashTransferRepository;
    private readonly ITransferRepository _transferRepository;
    private readonly IAccessTokenRepository _accessTokenRepository;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly AuthService _authService;
    private readonly string _subscriptionKey;

    public RemittanceController(
        ICashTransferRepository cashTransferRepository,
        ITransferRepository transferRepository,
        IAccessTokenRepository accessTokenRepository,
        HttpClient httpClient,
        IConfiguration config,
        AuthService authService)
    {
        _cashTransferRepository = cashTransferRepository;
        _transferRepository = transferRepository;
        _accessTokenRepository = accessTokenRepository;
        _httpClient = httpClient;
        _config = config;
        _authService = authService;
        _subscriptionKey = _config["MomoApi:SubscriptionKey:dummyRemittance"]; // Add subscription key initialization
    }

    // DTO for Access Token Response
    public class AccessTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    // Create Access Token
    [HttpPost("create-access-token")]
    public async Task<AccessTokenResponse> CreateAccessToken()
    {
        try
        {
            // Fetch Base64 encoded credentials
            var encodedCredentials = await _authService.GetEncodedCredentialsAsync();

            // Set up the HttpClient
            using var client = new HttpClient();

            // Request headers
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config["MomoApi:SubscriptionKey:dummyRemittance"]);

            // Prepare the URL for the request
            var url = $"{_config["MomoApi:BaseUrl"]}{_config["MomoApi:Remittance:CreateAccessToken"]}";

            // Make the POST request
            var response = await client.PostAsync(url, null);

            // Handle the response
            if (response.IsSuccessStatusCode)
            {
                var tokenJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AccessTokenResponse>(tokenJson); // Deserialize the token
            }

            // Log error response
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create access token. Response: {errorContent}");
        }
        catch (Exception ex)
        {
            // Log the exception message and stack trace
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            throw; // Rethrow exception for the calling method to handle
        }
    }
    // Get Basic User Info
    [HttpGet("get-basic-user-info/{accountHolderMSISDN}")]
    public async Task<IActionResult> GetBasicUserInfo(string accountHolderMSISDN)
    {
        try
        {
            // Call CreateAccessToken to get the token
            var accessTokenResponse = await CreateAccessToken(); // Now returns AccessTokenResponse directly

            var accessToken = accessTokenResponse.access_token; // Extract access token

            // Set up HttpClient for basic user info request
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Add("X-Target-Environment", "sandbox");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config["MomoApi:SubscriptionKey:dummyRemittance"]);

            // Prepare URL for basic user info request
            var url = $"{_config["MomoApi:BaseUrl"].TrimEnd('/')}/remittance/v1_0/accountholder/msisdn/{accountHolderMSISDN}/basicuserinfo";

            // Make GET request for basic user info
            var response = await client.GetAsync(url);

            // Handle the response
            if (response.IsSuccessStatusCode)
            {
                var userInfo = await response.Content.ReadAsStringAsync();
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = userInfo,
                    Message = "Basic user info retrieved successfully."
                });
            }

            // Log or handle specific error responses if needed
            var errorContent = await response.Content.ReadAsStringAsync();
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = $"Failed to retrieve basic user info. Response: {errorContent}"
            });
        }
        catch (Exception ex)
        {
            // Log and handle exceptions
            return StatusCode(500, new ApiResponse<string>
            {
                Success = false,
                Message = $"An error occurred: {ex.Message}"
            });
        }
    }
}
