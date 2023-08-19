using Common.DataAccess;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain;
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
