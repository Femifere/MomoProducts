namespace MomoProducts.Server.Repositories.Collections
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Collections;
    using MomoProducts.Server.Models.Collections;
    using System.Threading.Tasks;
    using MomoProducts.Server.Dtos.CollectionsDto;

    public class PreApprovalRepository : IPreApprovalRepository
    {
        private readonly AppDbContext _context;

        public PreApprovalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PreApprovalDto> GetPreApprovalByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<PreApprovalDto>().FirstOrDefaultAsync(pa => pa.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<PreApprovalDto>> GetAllPreApprovalsAsync()
        {
            return await _context.Set<PreApprovalDto>().ToListAsync();
        }

        public async Task CreatePreApprovalAsync(PreApprovalDto preApprovalDto)
        {
            _context.Set<PreApprovalDto>().Add(preApprovalDto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePreApprovalAsync(PreApprovalDto preApprovalDto)
        {
            // Check if the pre-approval exists in the database
            var existingPreApproval = await _context.Set<PreApprovalDto>().FirstOrDefaultAsync(pa => pa.ReferenceId == preApprovalDto.ReferenceId);

            if (existingPreApproval != null)
            {
                // Update the existing pre-approval with new values
                existingPreApproval.Status = preApprovalDto.Status;

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"PreApproval with ReferenceId {preApprovalDto.ReferenceId} not found.");
            }
        }
    }
}
