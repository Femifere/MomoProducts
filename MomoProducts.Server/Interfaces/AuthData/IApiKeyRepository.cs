using MomoProducts.Server.Models.AuthData;


namespace MomoProducts.Server.Interfaces.AuthData
{
    public interface IApiKeyRepository
    {
        Task<ApiKey> GetApiKeyAsync();
        Task<ApiKey> UpdateApiKeyAsync(ApiKey apiKey);
        Task<ApiKey> SaveApiKeyAsync(ApiKey apiKey);
        Task<ApiKey> GetLatestApiKeyAsync();
    }
}
