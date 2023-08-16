using Common.DataAccess;
using ItsCheck.Application.Interface;
using ItsCheck.Domain.Identity;
using ItsCheck.Persistence;

namespace ItsCheck.Application
{
    public class UserRepository : BaseRepository<User, ItsCheckContext>, IUserRepository
    {
        public UserRepository(ItsCheckContext context) : base(context)
        {
        }
    }
}
