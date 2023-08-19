using Common.DTO;
using ItsCheck.DTO;

namespace ItsCheck.Service.Interface
{
    public interface IChecklistItemService
    {
        Task<ResponseDTO> Create(ChecklistItemDTO checklistItemDTO);
        Task<ResponseDTO> Update(int id, ChecklistItemDTO checklistItemDTO);
        Task<ResponseDTO> Remove(int id);
        Task<ResponseDTO> GetList();
    }
}