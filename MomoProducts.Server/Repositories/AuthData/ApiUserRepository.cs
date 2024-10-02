namespace MomoProducts.Server.Repositories.AuthData
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.AuthData;
    using MomoProducts.Server.Models.AuthData;
    

    public class ApiUserRepository : IApiUserRepository
    {
        private readonly AppDbContext _context;

        public ApiUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiUser> GetApiUserAsync(string referenceId)
        {
            return await _context.Set<ApiUser>().FirstOrDefaultAsync(u => u.ReferenceId == referenceId);
        }

        public async Task<ApiUser> SaveApiUserAsync(ApiUser user)
        {
            var existingUser = await GetApiUserAsync(user.ReferenceId);

            if (existingUser == null)
            {
                await _context.Set<ApiUser>().AddAsync(user);
            }
            else
            {
                _context.Entry(existingUser).CurrentValues.SetValues(user);
            }

            await _context.SaveChangesAsync();
            return user;
        }
    }
}
