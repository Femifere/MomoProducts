using MomoProducts.Server.Models.Common;
using MomoProducts.Server.s.Common;

namespace MomoProducts.Server.Interfaces.Common
    {
        public interface IAccessTokenRepository
        {
            Task<AccessToken> GetAccessTokenAsync();
            Task SaveAccessTokenAsync(AccessToken token);
            
        }
    }

