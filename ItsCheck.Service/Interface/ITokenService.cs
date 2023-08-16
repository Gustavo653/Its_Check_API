
using ItsCheck.Domain.Identity;

namespace ItsCheck.Service.Interface
{
    public interface ITokenService
    {
        Task<string> CreateToken(User userDTO);
    }
}