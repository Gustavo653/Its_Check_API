using Common.DataAccess;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain;
using ItsCheck.Persistence;

namespace ItsCheck.DataAccess
{
    public class ChecklistRepository : BaseRepository<Checklist, ItsCheckContext>, IChecklistRepository
    {
        public ChecklistRepository(ItsCheckContext context) : base(context)
        {
        }
    }
}
