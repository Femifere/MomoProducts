using MomoProducts.Server.Interfaces.AuthData;
using MomoProducts.Server.Interfaces.Common;
using MomoProducts.Server.Repositories.AuthData;

namespace MomoProducts.Server.Services
{
    public class AuthService
    {
        private readonly IApiUserRepository _apiUserRepository;
        private readonly IApiKeyRepository _apiKeyRepository;
        private readonly IOauth2TokenRepository _oauth2TokenRepository;

        public AuthService(
            IApiUserRepository apiUserRepository,
            IApiKeyRepository apiKeyRepository,
            IOauth2TokenRepository oauth2TokenRepository)
        {
            _apiUserRepository = apiUserRepository;
            _apiKeyRepository = apiKeyRepository;
            _oauth2TokenRepository = oauth2TokenRepository;
        }

        // Method to get base64-encoded credentials (Basic Auth)
        public async Task<string> GetEncodedCredentialsAsync()
        {
            var apiUser = await _apiUserRepository.GetLatestApiUserAsync();
            var apiKey = await _apiKeyRepository.GetLatestApiKeyAsync(); // Use the new method

            if (apiUser == null || apiKey == null)
            {
                throw new Exception("API User or API Key not found.");
            }

            var credentials = $"{apiUser.ReferenceId}:{apiKey.APIKey}";
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
        }


        
    }
}