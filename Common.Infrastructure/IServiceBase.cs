using Common.DTO;

namespace Common.Infrastructure
{
    public interface IServiceBase<T>
    {
        Task<ResponseDTO> Create(T objectDTO);
        Task<ResponseDTO> Update(int id, T objectDTO);
        Task<ResponseDTO> Remove(int id);
        Task<ResponseDTO> GetList();
    }
}
