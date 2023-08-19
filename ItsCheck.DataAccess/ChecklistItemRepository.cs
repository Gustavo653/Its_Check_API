using Common.DataAccess;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain;
using ItsCheck.Persistence;

namespace ItsCheck.DataAccess
{
    public class ChecklistItemRepository : BaseRepository<ChecklistItem, ItsCheckContext>, IChecklistItemRepository
    {
        public ChecklistItemRepository(ItsCheckContext context) : base(context)
        {
        }
    }
}
