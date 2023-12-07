using ItsCheck.Domain;
using ItsCheck.Infrastructure.Repository;
using ItsCheck.Persistence;

namespace ItsCheck.DataAccess
{
    public class AmbulanceRepository : BaseRepository<Ambulance, ItsCheckContext>, IAmbulanceRepository
    {
        public AmbulanceRepository(ItsCheckContext context) : base(context)
        {
        }
    }
}
