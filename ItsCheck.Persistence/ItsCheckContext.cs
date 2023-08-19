using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ItsCheck.Domain;
using ItsCheck.Domain.Identity;

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
        public DbSet<ChecklistAdjustedItem> ChecklistAdjustedItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            }
           );
        }
    }
}
