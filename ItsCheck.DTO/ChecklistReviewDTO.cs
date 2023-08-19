using ItsCheck.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace ItsCheck.DTO
{
    public class ChecklistReviewDTO
    {
        [Required]
        public ReviewType Type { get; set; }
        [Required]
        public string Observation { get; set; }
        [Required]
        public int IdAmbulance { get; set; }
        [Required]
        public int IdChecklist { get; set; }
        [Required]
        public int IdUser { get; set; }
    }
}
