namespace MomoProducts.Server.Repositories.Collections
{
    using MomoProducts.Server.Interfaces.Collections;
    using MomoProducts.Server.Models.Collections;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class PreApprovalRepository : IPreApprovalRepository
    {
        private readonly DbContext _context;

        public PreApprovalRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<PreApproval> GetPreApprovalByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<PreApproval>().FirstOrDefaultAsync(pa => pa.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<PreApproval>> GetAllPreApprovalsAsync()
        {
            return await _context.Set<PreApproval>().ToListAsync();
        }

        public async Task CreatePreApprovalAsync(PreApproval preApproval)
        {
            _context.Set<PreApproval>().Add(preApproval);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePreApprovalAsync(PreApproval preApproval)
        {
            // Check if the pre-approval exists in the database
            var existingPreApproval = await _context.Set<PreApproval>().FirstOrDefaultAsync(pa => pa.ReferenceId == preApproval.ReferenceId);

            if (existingPreApproval != null)
            {
                // Update the existing pre-approval with new values
                existingPreApproval.Status = preApproval.Status;
                

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"PreApproval with ReferenceId {preApproval.ReferenceId} not found.");
            }
        }
    }
}
