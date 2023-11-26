using System.ComponentModel.DataAnnotations;

namespace ItsCheck.DTO
{
    public class ChecklistReplacedItemDTO
    {
        [Required] public int AmountReplaced { get; set; }
        [Required] public int IdItem { get; set; }
        [Required] public int IdChecklist { get; set; }
    }
}