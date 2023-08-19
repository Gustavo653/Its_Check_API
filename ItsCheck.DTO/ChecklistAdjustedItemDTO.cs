using System.ComponentModel.DataAnnotations;

namespace ItsCheck.DTO
{
    public class ChecklistAdjustedItemDTO
    {
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int IdItem { get; set; }
        [Required]
        public int IdChecklist { get; set; }
    }
}
