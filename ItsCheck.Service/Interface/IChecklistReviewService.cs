using Common.DTO;
using Common.Infrastructure;
using ItsCheck.DTO;

namespace ItsCheck.Service.Interface
{
    public interface IChecklistReviewService : IServiceBase<ChecklistReviewDTO>
    {
        Task<ResponseDTO> GetList(int? takeLast);
    }
}