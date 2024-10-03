using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MomoProducts.Server.Interfaces.Common;
using MomoProducts.Server.Models.Common;

namespace MomoProducts.Server.Repositories.AuthData
{
    public class Oauth2TokenRepository : IOauth2TokenRepository
    {
        private readonly AppDbContext _context;

        public Oauth2TokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Oauth2Token> GetOauth2TokenAsync()
        {
            return await _context.Set<Oauth2Token>()
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<Oauth2Token> SaveOauth2TokenAsync(Oauth2Token token)
        {
            await _context.Set<Oauth2Token>().AddAsync(token);
            await _context.SaveChangesAsync();
            return token;
        }
    }
}
