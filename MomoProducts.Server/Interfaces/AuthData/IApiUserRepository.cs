using MomoProducts.Server.Models.AuthData;
using MomoProducts.Server.s.AuthData;

namespace MomoProducts.Server.Interfaces.AuthData
{
    public interface IApiUserRepository
    {
        Task<ApiUser> GetApiUserAsync(string referenceId);
        Task<ApiUser> SaveApiUserAsync(ApiUser user);
    }
}
