using Common.DTO;
using ItsCheck.DTO;

namespace ItsCheck.Service.Interface
{
    public interface ICategoryService
    {
        Task<ResponseDTO> CreateCategory(BasicDTO basicDTO);
        Task<ResponseDTO> UpdateCategory(int id, BasicDTO basicDTO);
        Task<ResponseDTO> RemoveCategory(int id);
        Task<ResponseDTO> GetCategories();
    }
}