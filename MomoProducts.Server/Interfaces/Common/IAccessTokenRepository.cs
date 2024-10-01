using MomoProducts.Server.Models.Common;
using MomoProducts.Server.Dtos.CommonDto;

namespace MomoProducts.Server.Interfaces.Common
    {
        public interface IAccessTokenRepository
        {
            Task<AccessTokenDto> GetAccessTokenAsync();
            Task SaveAccessTokenAsync(AccessTokenDto tokenDto);
            
        }
    }

