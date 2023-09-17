using Common.DTO;
using Common.Infrastructure;
using ItsCheck.DTO;

namespace ItsCheck.Service.Interface
{
    public interface IChecklistService : IServiceBase<BasicDTO>
    {
        Task<ResponseDTO> Create(ChecklistDTO checklistDTO);
        Task<ResponseDTO> Update(int id, ChecklistDTO checklistDTO);
    }
}