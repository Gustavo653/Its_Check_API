using Microsoft.AspNetCore.Identity;

namespace ItsCheck.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public required string Name { get; set; }
        public Ambulance? Ambulance { get; set; }
        public virtual IEnumerable<UserRole> UserRoles { get; set; }
    }
}