using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Common.DataAccess
{
    public abstract class BaseRepository<TType, TContext>
            where TType : class, new()
            where TContext : DbContext
    {
        protected readonly TContext _dbContext;
        protected BaseRepository(TContext context) => _dbContext = context;
        protected TContext GetContext() => _dbContext;
        public virtual IQueryable<TType> GetEntities() => GetContext().Set<TType>()
                                                                      .AsNoTracking()
                                                                      .AsQueryable();
        public virtual IQueryable<TType> GetTrackedEntities() => GetContext().Set<TType>()
                                                                             .AsQueryable();
        public virtual async Task<IEnumerable<TType>> GetList() => await GetEntities().ToListAsync();
        public virtual void Insert(TType entity) => GetContext().Set<TType>()
                                                                .Add(entity);
        public virtual void InsertRange(IEnumerable<TType> entity) => GetContext().Set<TType>()
                                                                                  .AddRange(entity);
        public virtual async Task InsertAsync(TType entity) => await GetContext().Set<TType>()
                                                                                 .AddAsync(entity);
        public virtual void Update(TType entity) => GetContext().Set<TType>()
                                                                .Update(entity);
        public virtual void UpdateRange(TType[] entity) => GetContext().Set<TType>()
                                                                       .UpdateRange(entity);
        public virtual void Delete(TType entity) => GetContext().Set<TType>()
                                                                .Remove(entity);
        public virtual void DeleteRange(TType[] entity) => GetContext().Set<TType>()
                                                                .RemoveRange(entity);
        public virtual bool SaveChanges() => GetContext().SaveChanges() > 0;
        public virtual void Detach(TType entity) => GetContext().Entry(entity).State = EntityState.Detached;
        public virtual void Attach(TType entity) => GetContext().Set<TType>()
                                                                .Attach(entity);
        public virtual void AttachRange(IEnumerable<TType> entity) => GetContext().Set<TType>()
                                                                                  .AttachRange(entity);
        public virtual TType Clone(TType entity)
        {
            var ctx = GetContext();
            var value = ctx.Entry(entity).CurrentValues.Clone();
            var newEntity = new TType();
            ctx.Entry(newEntity).CurrentValues.SetValues(value);
            return newEntity;
        }

        public virtual IEnumerable<EntityEntry> GetChanges()
        {
            return GetContext().ChangeTracker.Entries();
        }

        public virtual void SeeChanges()
        {
            var change = GetContext().ChangeTracker.Entries();
        }

        public virtual void ClearChanges() => GetContext().ChangeTracker.Clear();

        public virtual async Task<bool> SaveChangesAsync() => await GetContext().SaveChangesAsync() > 0;
        public virtual async Task<List<TType>> GetListAsync() => await GetEntities().ToListAsync();
    }
}