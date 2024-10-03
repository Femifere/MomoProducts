using System.Threading.Tasks;
using MomoProducts.Server.Models.Common;

namespace MomoProducts.Server.Interfaces.Common
{
    public interface IOauth2TokenRepository
    {
        Task<Oauth2Token> GetOauth2TokenAsync();
        Task<Oauth2Token> SaveOauth2TokenAsync(Oauth2Token token);
    }
}
