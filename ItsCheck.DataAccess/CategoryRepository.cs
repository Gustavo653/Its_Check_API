using ItsCheck.Domain;
using ItsCheck.Infrastructure.Repository;
using ItsCheck.Persistence;

namespace ItsCheck.DataAccess
{
    public class CategoryRepository : BaseRepository<Category, ItsCheckContext>, ICategoryRepository
    {
        public CategoryRepository(ItsCheckContext context) : base(context)
        {
        }
    }
}
