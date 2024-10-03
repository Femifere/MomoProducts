using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MomoProducts.Server.Models.AuthData;
using MomoProducts.Server.Repositories.AuthData;
using Microsoft.Extensions.Configuration;
using MomoProducts.Server.Services;
using System;
using MomoProducts.Server.Dtos;
using MomoProducts.Server.Interfaces.AuthData;

namespace MomoProducts.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SandboxController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IApiUserRepository _apiUserRepository;
        private readonly IApiKeyRepository _apiKeyRepository;
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
        private readonly string _subscriptionKey;

        public SandboxController(
            IConfiguration configuration,
            IApiUserRepository apiUserRepository,
            IApiKeyRepository apiKeyRepository,
            AuthService authService,
            HttpClient httpClient)
        {
            _configuration = configuration;
            _apiUserRepository = apiUserRepository;
            _apiKeyRepository = apiKeyRepository;
            _httpClient = httpClient;
            _authService = authService;
            _subscriptionKey = _configuration["MomoApi:SubscriptionKey:dummyCollections"];
        }

        // POST: api/Sandbox/CreateUser
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser()
        {
            var baseUrl = _configuration["MomoApi:BaseUrl"];
            var endpoint = _configuration["MomoApi:SandboxUserProvisioning:CreateUser"];
            var url = $"{baseUrl}{endpoint}";

            // Generate UUID (X-Reference-Id)
            var referenceId = Guid.NewGuid().ToString().ToLower();

            // Build request message with headers
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);  // Add subscription key
            request.Headers.Add("X-Reference-Id", referenceId);

            request.Content = JsonContent.Create(new
            {
                providerCallbackHost = "Just a String"
            });

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Error creating API user: {errorContent}"
                });
            }

            var apiUser = new ApiUser
            {
                ReferenceId = referenceId,
                ProviderCallbackHost = "Just a String",
                CreatedDate = DateTime.Now // Assign current date and time
            };

            await _apiUserRepository.SaveApiUserAsync(apiUser);
            return Ok(new ApiResponse<ApiUser>
            {
                Success = true,
                Data = apiUser,
                Message = "User created successfully."
            });
        }

        // GET: api/Sandbox/GetUser/{referenceId}
        [HttpGet("GetUser/{referenceId}")]
        public async Task<IActionResult> GetUser(string referenceId)
        {
            var baseUrl = _configuration["MomoApi:BaseUrl"];
            var endpointTemplate = _configuration["MomoApi:SandboxUserProvisioning:GetUser"];
            var endpoint = endpointTemplate.Replace("{ReferenceId}", referenceId);
            var url = $"{baseUrl}{endpoint}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey); // Add subscription key

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Error fetching API user: {errorContent}"
                });
            }

            var apiUser = await response.Content.ReadFromJsonAsync<ApiUser>();
            if (apiUser != null)
            {
                return Ok(new ApiResponse<ApiUser>
                {
                    Success = true,
                    Data = apiUser,
                    Message = "User fetched successfully."
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = false,
                Message = "User not found."
            });
        }

        // POST: api/Sandbox/CreateAPIKey/{referenceId}
        [HttpPost("CreateAPIKey/{referenceId}")]
        public async Task<IActionResult> CreateAPIKey(string referenceId)
        {
            var baseUrl = _configuration["MomoApi:BaseUrl"];
            var endpointTemplate = _configuration["MomoApi:SandboxUserProvisioning:CreateAPIKey"];
            var endpoint = endpointTemplate.Replace("{ReferenceId}", referenceId);
            var url = $"{baseUrl}{endpoint}";

            // Build request message with headers
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("X-Reference-Id", referenceId);
            request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey); // Add subscription key

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Error creating API key: {errorContent}"
                });
            }

            var apiKey = await response.Content.ReadFromJsonAsync<ApiKey>();
            if (apiKey != null)
            {
                apiKey.CreatedDate = DateTime.Now; // Assign current date and time
                await _apiKeyRepository.SaveApiKeyAsync(apiKey);
                return Ok(new ApiResponse<ApiKey>
                {
                    Success = true,
                    Data = apiKey,
                    Message = "API key created successfully."
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = false,
                Message = "Failed to create API key."
            });
        }
    }
}
