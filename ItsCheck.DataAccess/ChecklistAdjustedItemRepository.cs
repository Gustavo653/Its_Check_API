using Common.DataAccess;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain;
using ItsCheck.Persistence;

namespace ItsCheck.DataAccess
{
    public class ChecklistAdjustedItemRepository : BaseRepository<ChecklistAdjustedItem, ItsCheckContext>, IChecklistAdjustedItemRepository
    {
        public ChecklistAdjustedItemRepository(ItsCheckContext context) : base(context)
        {
        }
    }
}
