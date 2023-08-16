using AutoMapper;
using ItsCheck.Domain.Identity;
using ItsCheck.DTO;

namespace ItsCheck.Service
{
    public class ItsCheckProfile : Profile
    {
        public ItsCheckProfile()
        {
            CreateMap<User, UserDTO>(MemberList.None).ReverseMap();
            CreateMap<User, UserLoginDTO>(MemberList.None).ReverseMap();
        }
    }
}