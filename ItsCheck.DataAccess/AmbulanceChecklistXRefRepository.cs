using ItsCheck.Domain;
using ItsCheck.Infrastructure.Repository;
using ItsCheck.Persistence;
using Microsoft.AspNetCore.Http;

namespace ItsCheck.DataAccess
{
    public class AmbulanceChecklistXRefRepository : TenantBaseRepository<AmbulanceChecklistXRef, ItsCheckContext>, IAmbulanceChecklistXRefRepository
    {
        public AmbulanceChecklistXRefRepository(IHttpContextAccessor httpContextAccessor, ItsCheckContext context) : base(httpContextAccessor, context)
        {
        }
    }
}
