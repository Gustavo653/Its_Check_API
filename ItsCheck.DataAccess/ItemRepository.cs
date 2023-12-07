using ItsCheck.Domain;
using ItsCheck.Infrastructure.Repository;
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
