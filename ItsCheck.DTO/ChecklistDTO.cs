using System.ComponentModel.DataAnnotations;
using ItsCheck.DTO.Base;

namespace ItsCheck.DTO
{
    public class ChecklistDTO : BasicDTO
    {
        [Required] 
        public virtual required IEnumerable<CategoryDTO> Categories { get; set; }
    }

    public class CategoryDTO
    {
        [Required] 
        public required int Id { get; set; }
        [Required] 
        public virtual required IEnumerable<ItemDTO> Items { get; set; }
    }

    public class ItemDTO
    {
        [Required] 
        public required int Id { get; set; }
        [Required] 
        public required int AmountRequired { get; set; }
    }
}