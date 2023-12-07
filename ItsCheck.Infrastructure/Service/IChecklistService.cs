using ItsCheck.DTO;
using ItsCheck.DTO.Base;
using ItsCheck.Infrastructure.Base;

namespace ItsCheck.Infrastructure.Service
{
    public interface IChecklistService : IBaseService<BasicDTO>
    {
        Task<ResponseDTO> Create(ChecklistDTO checklistDTO);
        Task<ResponseDTO> Update(int id, ChecklistDTO checklistDTO);
        Task<ResponseDTO> GetById(int id);
        Task<ResponseDTO> GetByAmbulanceId(int id);
    }
}