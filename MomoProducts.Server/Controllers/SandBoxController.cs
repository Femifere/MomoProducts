using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MomoProducts.Server.Models.AuthData;
using MomoProducts.Server.Repositories.AuthData;
using Microsoft.Extensions.Configuration;

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

        public SandboxController(IConfiguration configuration,
                                ApiUserRepository apiUserRepository,
                                ApiKeyRepository apiKeyRepository,
                                HttpClient httpClient)
        {
            _configuration = configuration;
            _apiUserRepository = apiUserRepository;
            _apiKeyRepository = apiKeyRepository;
            _httpClient = httpClient;
        }

        // POST: api/Sandbox/CreateUser
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser()
        {
            var url = _configuration["SandboxUserProvisioning:CreateUser"];
            var response = await _httpClient.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error creating API user.");
            }

            var apiUser = await response.Content.ReadFromJsonAsync<ApiUser>();
            if (apiUser != null)
            {
               await _apiUserRepository.SaveApiUserAsync(apiUser);
                return Ok(apiUser);
            }

            return BadRequest("Failed to create user.");
        }

        // GET: api/Sandbox/GetUser/{referenceId}
        [HttpGet("GetUser/{referenceId}")]
        public async Task<IActionResult> GetUser(string referenceId)
        {
            var url = _configuration["SandboxUserProvisioning:GetUser"].Replace("{ReferenceId}", referenceId);
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error fetching API user.");
            }

            var apiUser = await response.Content.ReadFromJsonAsync<ApiUser>();
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
            var url = _configuration["SandboxUserProvisioning:CreateAPIKey"].Replace("{ReferenceId}", referenceId);
            var response = await _httpClient.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error creating API key.");
            }

            var apiKey = await response.Content.ReadFromJsonAsync<ApiKey>();
            if (apiKey != null)
            {
                await _apiKeyRepository.SaveApiKeyAsync(apiKey);
                return Ok(apiKey);
            }

            return BadRequest("Failed to create API key.");
        }
    }
}
