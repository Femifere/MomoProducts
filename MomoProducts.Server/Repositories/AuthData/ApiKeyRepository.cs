namespace MomoProducts.Server.Repositories.AuthData
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.AuthData;
    using MomoProducts.Server.Models.AuthData;
    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly AppDbContext _context;

        public ApiKeyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiKey> GetApiKeyAsync()
        {
            return await _context.Set<ApiKey>().FirstOrDefaultAsync();
        }

        public async Task<ApiKey> SaveApiKeyAsync(ApiKey apiKey)
        {
            await _context.Set<ApiKey>().AddAsync(apiKey);
            await _context.SaveChangesAsync();
            return apiKey;
        }

        public async Task<ApiKey> UpdateApiKeyAsync(ApiKey apiKey)
        {
            var existingApiKey = await GetApiKeyAsync();

            if (existingApiKey != null)
            {
                _context.Entry(existingApiKey).CurrentValues.SetValues(apiKey);
                await _context.SaveChangesAsync();
            }

            return apiKey;
        }
    }
}
