using MomoProducts.Server.Models.AuthData;

namespace MomoProducts.Server.Interfaces.AuthData
{
    public interface IApiKeyRepository
    {
        Task<ApiKey> GetApiKeyAsync();
    }
}
