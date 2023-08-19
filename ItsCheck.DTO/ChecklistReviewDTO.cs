using ItsCheck.Domain.Identity;
using ItsCheck.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItsCheck.DTO
{
    public class ChecklistReviewDTO
    {
        [Required]
        public int Type { get; set; }
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
