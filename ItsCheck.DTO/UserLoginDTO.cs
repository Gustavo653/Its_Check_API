using System.ComponentModel.DataAnnotations;

namespace ItsCheck.DTO
{
    public class UserLoginDTO
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}