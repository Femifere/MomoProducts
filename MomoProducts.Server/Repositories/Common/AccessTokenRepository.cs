namespace MomoProducts.Server.Repositories.Common
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Common;
    using MomoProducts.Server.Models.Common;
    using System;

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

        public async Task<AccessToken> SaveAccessTokenAsync(AccessToken token)
        {
            var existingToken = await GetAccessTokenAsync();

            if (existingToken == null)
            {
                // Add the new token
                await _context.Set<AccessToken>().AddAsync(token);
            }
            else
            {
                // Update the existing token
                _context.Entry(existingToken).CurrentValues.SetValues(token);
            }

            await _context.SaveChangesAsync();
            return token; // Return the saved or updated token
        }



        public bool IsTokenExpired(AccessToken token)
        {
            return DateTime.UtcNow >= token.ExpiresIn; // Check if the token is expired
        }
    }
}
