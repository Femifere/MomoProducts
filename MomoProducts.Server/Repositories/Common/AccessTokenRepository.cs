namespace MomoProducts.Server.Repositories.Common
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Common;
    using MomoProducts.Server.Models.Common;
    using MomoProducts.Server.Dtos.CommonDto;

    public class AccessTokenRepository : IAccessTokenRepository
    {
        private readonly AppDbContext _context;

        public AccessTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AccessTokenDto> GetAccessTokenAsync()
        {
            return await _context.Set<AccessTokenDto>().FirstOrDefaultAsync();
        }

        public async Task SaveAccessTokenAsync(AccessTokenDto tokenDto)
        {
            var existingToken = await GetAccessTokenAsync();
            if (existingToken != null)
            {
                existingToken.accessToken = tokenDto.accessToken;
                existingToken.ExpiresIn = tokenDto.ExpiresIn;
                _context.Set<AccessTokenDto>().Update(existingToken);
            }
            else
            {
                await _context.Set<AccessTokenDto>().AddAsync(tokenDto);
            }
            await _context.SaveChangesAsync();
        }
    }
}
