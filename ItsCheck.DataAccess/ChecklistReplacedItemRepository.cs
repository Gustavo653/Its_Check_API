using ItsCheck.Domain;
using ItsCheck.Infrastructure.Repository;
using ItsCheck.Persistence;

namespace ItsCheck.DataAccess
{
    public class ChecklistReplacedItemRepository : BaseRepository<ChecklistReplacedItem, ItsCheckContext>, IChecklistReplacedItemRepository
    {
        public ChecklistReplacedItemRepository(ItsCheckContext context) : base(context)
        {
        }
    }
}
