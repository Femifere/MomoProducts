namespace MomoProducts.Server.Repositories.AuthData
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.AuthData;
    using MomoProducts.Server.Models.AuthData;
    using MomoProducts.Server.Dtos.AuthDataDto;

    public class ApiUserRepository : IApiUserRepository
    {
        private readonly AppDbContext _context;

        public ApiUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiUserDto> GetApiUserAsync(string referenceId)
        {
            return await _context.Set<ApiUserDto>().FirstOrDefaultAsync(u => u.ReferenceId == referenceId);
        }

        public async Task<ApiUserDto> SaveApiUserAsync(ApiUserDto userDto)
        {
            var existingUser = await GetApiUserAsync(userDto.ReferenceId);

            if (existingUser == null)
            {
                await _context.Set<ApiUserDto>().AddAsync(userDto);
            }
            else
            {
                _context.Entry(existingUser).CurrentValues.SetValues(userDto);
            }

            await _context.SaveChangesAsync();
            return userDto;
        }
    }
}
