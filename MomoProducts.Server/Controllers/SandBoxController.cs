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
using MomoProducts.Server.Dtos.AuthDataDto;

namespace MomoProducts.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SandboxController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApiUserRepository _apiUserRepository;
        private readonly ApiKeyRepository _apiKeyRepository;
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public SandboxController(IConfiguration configuration,
                                ApiUserRepository apiUserRepository,
                                ApiKeyRepository apiKeyRepository,
                                AuthService authService,
                                HttpClient httpClient)
        {
            _configuration = configuration;
            _apiUserRepository = apiUserRepository;
            _apiKeyRepository = apiKeyRepository;
            _httpClient = httpClient;
            _authService = authService;
        }

        // POST: api/Sandbox/CreateUser
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser()
        {
            var url = _configuration["SandboxUserProvisioning:CreateUser"];

            // Generate UUID (X-Reference-Id)
            var referenceId = Guid.NewGuid().ToString();

            // Build request message with headers
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("X-Reference-Id", referenceId);
            request.Content = JsonContent.Create(new
            {
                providerCallbackHost = _configuration["ProviderCallbackHost"]
            });

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error creating API user.");
            }

            var apiUser = new ApiUserDto
            {
                ReferenceId = referenceId,
                ProviderCallbackHost = _configuration["ProviderCallbackHost"]
            };

            await _apiUserRepository.SaveApiUserAsync(apiUser);
            return Ok(apiUser);
        }

        // GET: api/Sandbox/GetUser/{referenceId}
        [HttpGet("GetUser/{referenceId}")]
        public async Task<IActionResult> GetUser(string referenceId)
        {
            var url = _configuration["SandboxUserProvisioning:GetUser"]
                        .Replace("{X-Reference-Id}", referenceId);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error fetching API user.");
            }

            var apiUser = await response.Content.ReadFromJsonAsync<ApiUserDto>();
            if (apiUser != null)
            {
                return Ok(apiUser);
            }

            return NotFound("User not found.");
        }

        // POST: api/Sandbox/CreateAPIKey/{referenceId}
        [HttpPost("CreateAPIKey/{referenceId}")]
        public async Task<IActionResult> CreateAPIKey(string referenceId)
        {
            var url = _configuration["SandboxUserProvisioning:CreateAPIKey"]
                        .Replace("{X-Reference-Id}", referenceId);

            // Build request message with headers
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("X-Reference-Id", referenceId);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error creating API key.");
            }

            var apiKey = await response.Content.ReadFromJsonAsync<ApiKeyDto>();
            if (apiKey != null)
            {
                await _apiKeyRepository.SaveApiKeyAsync(apiKey);
                return Ok(apiKey);
            }

            return BadRequest("Failed to create API key.");
        }
    }
}
