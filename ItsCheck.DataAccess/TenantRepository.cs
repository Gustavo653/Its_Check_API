using ItsCheck.Domain;
using ItsCheck.Infrastructure.Repository;
using ItsCheck.Persistence;
using Microsoft.AspNetCore.Http;

namespace ItsCheck.DataAccess
{
    public class TenantRepository : BaseRepository<Tenant, ItsCheckContext>, ITenantRepository
    {
        public TenantRepository(ItsCheckContext context) : base(context)
        {
        }
    }
}
