using MomoProducts.Server.Models.AuthData;
using MomoProducts.Server.Dtos.AuthDataDto;

namespace MomoProducts.Server.Interfaces.AuthData
{
    public interface IApiKeyRepository
    {
        Task<ApiKeyDto> GetApiKeyAsync();
        Task<ApiKeyDto> UpdateApiKeyAsync(ApiKeyDto apiKey);
        Task<ApiKeyDto> SaveApiKeyAsync(ApiKeyDto apiKey);
    }
}
