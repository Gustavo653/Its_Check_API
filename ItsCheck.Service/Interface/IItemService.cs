using Common.DTO;
using ItsCheck.DTO;

namespace ItsCheck.Service.Interface
{
    public interface IItemService
    {
        Task<ResponseDTO> Create(BasicDTO basicDTO);
        Task<ResponseDTO> Update(int id, BasicDTO basicDTO);
        Task<ResponseDTO> Remove(int id);
        Task<ResponseDTO> GetList();
    }
}