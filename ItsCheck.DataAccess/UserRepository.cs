using Common.DataAccess;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain.Identity;
using ItsCheck.Persistence;

namespace ItsCheck.DataAccess
{
    public class UserRepository : BaseRepository<User, ItsCheckContext>, IUserRepository
    {
        public UserRepository(ItsCheckContext context) : base(context)
        {
        }
    }
}
