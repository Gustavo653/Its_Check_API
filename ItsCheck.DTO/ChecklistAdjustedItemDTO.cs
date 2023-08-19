using ItsCheck.Domain.Identity;
using ItsCheck.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItsCheck.Domain.Enum;

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
