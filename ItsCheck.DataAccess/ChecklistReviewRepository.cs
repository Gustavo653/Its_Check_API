using Common.DataAccess;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain;
using ItsCheck.Persistence;

namespace ItsCheck.DataAccess
{
    public class ChecklistReviewRepository : BaseRepository<ChecklistReview, ItsCheckContext>, IChecklistReviewRepository
    {
        public ChecklistReviewRepository(ItsCheckContext context) : base(context)
        {
        }
    }
}
