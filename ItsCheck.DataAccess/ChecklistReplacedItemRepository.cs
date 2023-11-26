using Common.DataAccess;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain;
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
