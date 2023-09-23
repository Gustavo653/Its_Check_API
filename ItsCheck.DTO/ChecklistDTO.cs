using System.ComponentModel.DataAnnotations;

namespace ItsCheck.DTO
{
    public class ChecklistDTO : BasicDTO
    {
        [Required]
        public required virtual IEnumerable<CategoryDTO> Categories { get; set; }
    }
    public class CategoryDTO
    {
        [Required]
        public required int Id { get; set; }
        [Required]
        public required virtual IEnumerable<ItemDTO> Items { get; set; }
    }
    public class ItemDTO
    {
        [Required]
        public required int Id { get; set; }
        [Required]
        public required int Quantity { get; set; }
        [Required]
        public required int QuantityReplenished { get; set; }
    }
}
