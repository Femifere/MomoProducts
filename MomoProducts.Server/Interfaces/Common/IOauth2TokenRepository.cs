using MomoProducts.Server.Models.Common;
using MomoProducts.Server.Dtos.CommonDto;
namespace MomoProducts.Server.Interfaces.Common
{
    public interface IOauth2TokenRepository
    {
        Task<Oauth2TokenDto> GetOauth2TokenAsync();
        Task SaveOauth2TokenAsync(Oauth2TokenDto tokenDto);
    }
}

