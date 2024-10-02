namespace MomoProducts.Server.Repositories.Common
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Common;
    using MomoProducts.Server.Models.Common;


    public class AccessTokenRepository : IAccessTokenRepository
    {
        private readonly AppDbContext _context;

        public AccessTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AccessToken> GetAccessTokenAsync()
        {
            return await _context.Set<AccessToken>().FirstOrDefaultAsync();
        }

        public async Task SaveAccessTokenAsync(AccessToken token)
        {
            var existingToken = await GetAccessTokenAsync();
            if (existingToken != null)
            {
                existingToken.accessToken = token.accessToken;
                existingToken.ExpiresIn = token.ExpiresIn;
                _context.Set<AccessToken>().Update(existingToken);
            }
            else
            {
                await _context.Set<AccessToken>().AddAsync(token);
            }
            await _context.SaveChangesAsync();
        }
    }
}
