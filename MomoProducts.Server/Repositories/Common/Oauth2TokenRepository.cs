using Microsoft.EntityFrameworkCore;
using MomoProducts.Server.Interfaces.Common;
using MomoProducts.Server.Models.Common;

namespace MomoProducts.Server.Repositories.Common
{
    public class Oauth2TokenRepository : IOauth2TokenRepository
    {
        private readonly DbContext _context;

        public Oauth2TokenRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Oauth2Token> GetOauth2TokenAsync()
        {
            return await _context.Set<Oauth2Token>().FirstOrDefaultAsync();
        }

        public async Task SaveOauth2TokenAsync(Oauth2Token token)
        {
            var existingToken = await GetOauth2TokenAsync();
            if (existingToken != null)
            {
                existingToken.AccessToken = token.AccessToken;
                existingToken.ExpiresIn = token.ExpiresIn;
                _context.Set<Oauth2Token>().Update(existingToken);
            }
            else
            {
                await _context.Set<Oauth2Token>().AddAsync(token);
            }
            await _context.SaveChangesAsync();
        }
    }
}
