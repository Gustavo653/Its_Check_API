using Common.DTO;
using ItsCheck.DTO;

namespace ItsCheck.Service.Interface
{
    public interface IAmbulanceService
    {
        Task<ResponseDTO> Create(AmbulanceDTO ambulanceDTO);
        Task<ResponseDTO> Update(int id, AmbulanceDTO ambulanceDTO);
        Task<ResponseDTO> Remove(int id);
        Task<ResponseDTO> GetList();
    }
}