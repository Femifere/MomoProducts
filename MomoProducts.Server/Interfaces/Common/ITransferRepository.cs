using MomoProducts.Server.Models.Common;
using MomoProducts.Server.Dtos.CommonDto;
namespace MomoProducts.Server.Interfaces.Common
{
    public interface ITransferRepository
    {
        Task<TransferDto> GetTransferByReferenceIdAsync(string referenceId);
        Task<IEnumerable<TransferDto>> GetAllTransfersAsync();
        Task CreateTransferAsync(TransferDto transferDto);
    }
}
