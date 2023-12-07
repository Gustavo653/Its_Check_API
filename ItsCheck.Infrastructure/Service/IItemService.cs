using ItsCheck.DTO.Base;
using ItsCheck.Infrastructure.Base;
using Microsoft.AspNetCore.Http;

namespace ItsCheck.Infrastructure.Service
{
    public interface IItemService : IBaseService<BasicDTO>
    {
        Task<ResponseDTO> ImportCSV(IFormFile csvFile);
    }
}