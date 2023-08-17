using Common.DataAccess;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain;
using ItsCheck.Persistence;

namespace ItsCheck.DataAccess
{
    public class ItemRepository : BaseRepository<Item, ItsCheckContext>, IItemRepository
    {
        public ItemRepository(ItsCheckContext context) : base(context)
        {
        }
    }
}
