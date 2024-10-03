using MomoProducts.Server.Models.Common;


namespace MomoProducts.Server.Interfaces.Common
    {
        public interface IAccessTokenRepository
        {
            Task<AccessToken> GetAccessTokenAsync();

        Task<AccessToken> SaveAccessTokenAsync(AccessToken token);
            public bool IsTokenExpired(AccessToken token);
        }
    }

