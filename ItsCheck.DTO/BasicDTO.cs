using System.ComponentModel.DataAnnotations;

namespace ItsCheck.DTO
{
    public class BasicDTO
    {
        [Required]
        public required string Name { get; set; }
    }
}