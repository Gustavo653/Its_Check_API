﻿using ItsCheck.Domain;
using ItsCheck.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ItsCheck.DataAccess
{
    public abstract class TenantBaseRepository<TType, TContext> : BaseRepository<TType, TContext> where TType : TenantBaseEntity where TContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        protected TenantBaseRepository(IHttpContextAccessor httpContextAccessor, TContext context) : base(context)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task InsertAsync(TType entity)
        {
            _session.TryGetValue(Consts.ClaimTenantId, out byte[] tenantId);
            entity.TenantId = Convert.ToInt32(Encoding.UTF8.GetString(tenantId));
            return base.InsertAsync(entity);
        }
    }
}