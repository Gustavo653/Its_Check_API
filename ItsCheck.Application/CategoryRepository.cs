using Common.DataAccess;
using ItsCheck.Application.Interface;
using ItsCheck.Domain;
using ItsCheck.Persistence;

namespace ItsCheck.Application
{
    public class CategoryRepository : BaseRepository<Category, ItsCheckContext>, ICategoryRepository
    {
        public CategoryRepository(ItsCheckContext context) : base(context)
        {
        }
    }
}
