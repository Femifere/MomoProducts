namespace MomoProducts.Server.Repositories.AuthData
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.AuthData;
    using MomoProducts.Server.Models.AuthData;
    using MomoProducts.Server.Dtos.AuthDataDto;
    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly AppDbContext _context;

        public ApiKeyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiKeyDto> GetApiKeyAsync()
        {
            return await _context.Set<ApiKeyDto>().FirstOrDefaultAsync();
        }

        public async Task<ApiKeyDto> SaveApiKeyAsync(ApiKeyDto apiKeyDto)
        {
            await _context.Set<ApiKeyDto>().AddAsync(apiKeyDto);
            await _context.SaveChangesAsync();
            return apiKeyDto;
        }

        public async Task<ApiKeyDto> UpdateApiKeyAsync(ApiKeyDto apiKeyDto)
        {
            var existingApiKey = await GetApiKeyAsync();

            if (existingApiKey != null)
            {
                _context.Entry(existingApiKey).CurrentValues.SetValues(apiKeyDto);
                await _context.SaveChangesAsync();
            }

            return apiKeyDto;
        }
    }
}
