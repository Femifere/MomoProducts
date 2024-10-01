using MomoProducts.Server.Models.AuthData;
using MomoProducts.Server.Dtos.AuthDataDto;

namespace MomoProducts.Server.Interfaces.AuthData
{
    public interface IApiUserRepository
    {
        Task<ApiUserDto> GetApiUserAsync(string referenceId);
        Task<ApiUserDto> SaveApiUserAsync(ApiUserDto user);
    }
}
