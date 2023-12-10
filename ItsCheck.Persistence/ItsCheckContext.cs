using ItsCheck.Domain;
using ItsCheck.Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ItsCheck.Persistence
{
    public class ItsCheckContext : IdentityDbContext<User, Role, int,
                                               IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                                               IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public ItsCheckContext(DbContextOptions<ItsCheckContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected ItsCheckContext()
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<ChecklistItem> ChecklistItems { get; set; }
        public DbSet<Ambulance> Ambulances { get; set; }
        public DbSet<ChecklistReview> ChecklistReviews { get; set; }
        public DbSet<ChecklistReplacedItem> ChecklistReplacedItems { get; set; }

        private string? GetTenantId()
        {
            _session.TryGetValue("tenantId", out byte[] tenantId);
            if (tenantId == null)
                return null;
            return Encoding.UTF8.GetString(tenantId);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(x =>
            {
                x.HasKey(ur => new { ur.UserId, ur.RoleId });

                x.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                x.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<Ambulance>(x =>
            {
                x.HasIndex(nameof(Ambulance.Number), $"{nameof(Ambulance.Checklist)}Id").IsUnique();
                x.HasQueryFilter(a => a.TenantId.ToString() == (GetTenantId() ?? a.TenantId.ToString()));
            });

            modelBuilder.Entity<Checklist>(x =>
            {
                x.HasIndex(a => a.Name).IsUnique();
                x.HasQueryFilter(a => a.TenantId.ToString() == (GetTenantId() ?? a.TenantId.ToString()));
            });

            modelBuilder.Entity<ChecklistItem>(x =>
            {
                x.HasIndex($"{nameof(ChecklistItem.Item)}Id", $"{nameof(ChecklistItem.Category)}Id", $"{nameof(ChecklistItem.Checklist)}Id").IsUnique();
                x.HasQueryFilter(a => a.TenantId.ToString() == (GetTenantId() ?? a.TenantId.ToString()));
            });

            modelBuilder.Entity<Category>(x =>
            {
                x.HasIndex(a => a.Name).IsUnique();
                x.HasQueryFilter(a => a.TenantId.ToString() == (GetTenantId() ?? a.TenantId.ToString()));
            });

            modelBuilder.Entity<Item>(x =>
            {
                x.HasIndex(a => a.Name).IsUnique();
                x.HasQueryFilter(a => a.TenantId.ToString() == (GetTenantId() ?? a.TenantId.ToString()));
            });

            modelBuilder.Entity<Tenant>(x =>
            {
                x.HasIndex(a => a.Name).IsUnique();
            });

            modelBuilder.Entity<ChecklistReview>(x =>
            {
                x.HasQueryFilter(a => a.TenantId.ToString() == (GetTenantId() ?? a.TenantId.ToString()));
            });

            modelBuilder.Entity<ChecklistReplacedItem>(x =>
            {
                x.HasIndex($"{nameof(ChecklistReplacedItem.ChecklistItem)}Id", $"{nameof(ChecklistReplacedItem.ChecklistReview)}Id").IsUnique();
                x.HasQueryFilter(a => a.TenantId.ToString() == (GetTenantId() ?? a.TenantId.ToString()));
            });
        }
    }
}
