using System;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MomoProducts.Server.Interfaces.Common;
using MomoProducts.Server.Repositories.AuthData;
using MomoProducts.Server.Services;
using MomoProducts.Server.Interfaces.AuthData;
using MomoProducts.Server.Models.AuthData;
using MomoProducts.Server.Models.Common;

namespace MomoProducts.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IApiUserRepository> _apiUserRepositoryMock; // Assuming IApiUserRepository interface exists
        private readonly Mock<IApiKeyRepository> _apiKeyRepositoryMock;   // Assuming IApiKeyRepository interface exists
        private readonly Mock<IOauth2TokenRepository> _oauth2TokenRepositoryMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _apiUserRepositoryMock = new Mock<IApiUserRepository>();
            _apiKeyRepositoryMock = new Mock<IApiKeyRepository>();
            _oauth2TokenRepositoryMock = new Mock<IOauth2TokenRepository>();
            _authService = new AuthService(
                _apiUserRepositoryMock.Object,
                _apiKeyRepositoryMock.Object,
                _oauth2TokenRepositoryMock.Object);
        }

        [Fact]
        public async Task GetEncodedCredentialsAsync_ReturnsEncodedCredentials_WhenApiUserAndApiKeyExist()
        {
            // Arrange
            var apiUser = new ApiUser { ReferenceId = "user123" }; // Ensure you have a class ApiUser
            var apiKey = new ApiKey { APIKey = "key456" }; // Ensure you have a class ApiKey
            _apiUserRepositoryMock.Setup(repo => repo.GetLatestApiUserAsync()).ReturnsAsync(apiUser);
            _apiKeyRepositoryMock.Setup(repo => repo.GetApiKeyAsync()).ReturnsAsync(apiKey);

            // Act
            var result = await _authService.GetEncodedCredentialsAsync();

            // Assert
            var expectedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiUser.ReferenceId}:{apiKey.APIKey}"));
            Assert.Equal(expectedCredentials, result);
        }

        [Fact]
        public async Task GetEncodedCredentialsAsync_ThrowsException_WhenApiUserIsNull()
        {
            // Arrange
            _apiUserRepositoryMock.Setup(repo => repo.GetLatestApiUserAsync()).ReturnsAsync((ApiUser)null);
            _apiKeyRepositoryMock.Setup(repo => repo.GetApiKeyAsync()).ReturnsAsync(new ApiKey { APIKey = "key456" });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _authService.GetEncodedCredentialsAsync());
            Assert.Equal("API User or API Key not found.", exception.Message);
        }

        [Fact]
        public async Task GetLatestOauth2TokenWithBearerAsync_ReturnsAccessToken_WhenTokenIsValid()
        {
            // Arrange
            var token = new Oauth2Token { AccessToken = "token123" }; // Ensure you have a class Oauth2Token
            _oauth2TokenRepositoryMock.Setup(repo => repo.GetOauth2TokenAsync()).ReturnsAsync(token);

            // Act
            var result = await _authService.GetLatestOauth2TokenWithBearerAsync();

            // Assert
            Assert.Equal(token.AccessToken, result);
        }

        [Fact]
        public async Task GetLatestOauth2TokenWithBearerAsync_ThrowsException_WhenTokenIsInvalid()
        {
            // Arrange
            _oauth2TokenRepositoryMock.Setup(repo => repo.GetOauth2TokenAsync()).ReturnsAsync((Oauth2Token)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _authService.GetLatestOauth2TokenWithBearerAsync());
            Assert.Equal("No valid OAuth2 token found.", exception.Message);
        }
    }
}
