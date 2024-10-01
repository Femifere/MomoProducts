namespace MomoProducts.Server.Repositories.Common
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Common;
    using MomoProducts.Server.Models.Common;
    using MomoProducts.Server.Dtos.CommonDto;

    public class MoneyRepository : IMoneyRepository
    {
        private readonly AppDbContext _context;

        public MoneyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MoneyDto> GetMoneyAsync()
        {
            return await _context.Set<MoneyDto>().FirstOrDefaultAsync();
        }
    }

}
