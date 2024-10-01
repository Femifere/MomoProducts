using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MomoProducts.Server.Repositories.AuthData;
using MomoProducts.Server.Interfaces.Common; // Ensure the interface for token repository is included

namespace MomoProducts.Server.Services
{
    public class AuthService
    {
        private readonly ApiUserRepository _apiUserRepository;
        private readonly ApiKeyRepository _apiKeyRepository;
        private readonly IOauth2TokenRepository _oauth2TokenRepository; // Add this line

        public AuthService(ApiUserRepository apiUserRepository, ApiKeyRepository apiKeyRepository, IOauth2TokenRepository oauth2TokenRepository) // Modify constructor
        {
            _apiUserRepository = apiUserRepository;
            _apiKeyRepository = apiKeyRepository;
            _oauth2TokenRepository = oauth2TokenRepository; // Initialize the repository
        }

        public async Task<string> GetEncodedCredentialsAsync()
        {
            var apiUser = await _apiUserRepository.GetApiUserAsync("some_reference_id"); // Replace with actual reference ID logic
            var apiKey = await _apiKeyRepository.GetApiKeyAsync();

            if (apiUser == null || apiKey == null)
            {
                throw new Exception("API User or API Key not found.");
            }

            var credentials = $"{apiUser.ReferenceId}:{apiKey.APIKey}"; // Assuming UserId and Key are the properties
            var encodedCredentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));

            return encodedCredentials;
        }

        // New function to get the latest OAuth2 token with "Bearer"
        public async Task<string> GetLatestOauth2TokenWithBearerAsync()
        {
            var token = await _oauth2TokenRepository.GetOauth2TokenAsync();
            if (token == null || string.IsNullOrEmpty(token.AccessToken))
            {
                throw new Exception("No valid OAuth2 token found.");
            }
            return $"Bearer {token.AccessToken}";
        }
    }
}
