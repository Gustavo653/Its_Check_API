using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItsCheck.DTO
{
    public class AmbulanceDTO
    {
        [Required]
        public int Number { get; set; }
        [Required]
        public int IdChecklist { get; set; }
    }
}
