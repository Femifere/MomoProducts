namespace MomoProducts.Server.Repositories.Common
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Common;
    using MomoProducts.Server.Models.Common;
    using MomoProducts.Server.Dtos.CommonDto;

    public class Oauth2TokenRepository : IOauth2TokenRepository
    {
        private readonly AppDbContext _context;

        public Oauth2TokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Oauth2TokenDto> GetOauth2TokenAsync()
        {
            return await _context.Set<Oauth2TokenDto>().FirstOrDefaultAsync();
        }

        public async Task SaveOauth2TokenAsync(Oauth2TokenDto tokenDto)
        {
            var existingToken = await GetOauth2TokenAsync();
            if (existingToken != null)
            {
                existingToken.AccessToken = tokenDto.AccessToken;
                existingToken.ExpiresIn = tokenDto.ExpiresIn;
                _context.Set<Oauth2TokenDto>().Update(existingToken);
            }
            else
            {
                await _context.Set<Oauth2TokenDto>().AddAsync(tokenDto);
            }
            await _context.SaveChangesAsync();
        }
    }
}
