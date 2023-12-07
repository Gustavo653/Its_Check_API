using ItsCheck.Domain;
using ItsCheck.Infrastructure.Repository;
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
