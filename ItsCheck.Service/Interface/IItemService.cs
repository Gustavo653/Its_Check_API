using Common.DTO;
using Common.Infrastructure;
using ItsCheck.DTO;
using Microsoft.AspNetCore.Http;

namespace ItsCheck.Service.Interface
{
    public interface IItemService : IServiceBase<BasicDTO>
    {
        Task<ResponseDTO> ImportCSV(IFormFile csvFile);
    }
}