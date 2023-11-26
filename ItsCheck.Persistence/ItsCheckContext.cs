using ItsCheck.Domain;
using ItsCheck.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ItsCheck.Persistence
{
    public class ItsCheckContext : IdentityDbContext<User, Role, int,
                                               IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                                               IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ItsCheckContext(DbContextOptions<ItsCheckContext> options) : base(options) { }

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
            });

            modelBuilder.Entity<Checklist>(x =>
            {
                x.HasIndex(a => a.Name).IsUnique();
            });

            modelBuilder.Entity<ChecklistItem>(x =>
            {
                x.HasIndex($"{nameof(ChecklistItem.Item)}Id", $"{nameof(ChecklistItem.Category)}Id", $"{nameof(ChecklistItem.Checklist)}Id").IsUnique();
            });

            modelBuilder.Entity<Category>(x =>
            {
                x.HasIndex(a => a.Name).IsUnique();
            });

            modelBuilder.Entity<Item>(x =>
            {
                x.HasIndex(a => a.Name).IsUnique();
            });

            modelBuilder.Entity<ChecklistReplacedItem>(x =>
            {
                x.HasIndex($"{nameof(ChecklistReplacedItem.ChecklistItem)}Id", $"{nameof(ChecklistReplacedItem.ChecklistReview)}Id").IsUnique();
            });
        }
    }
}
