﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItsCheck.DTO
{
    public class ChecklistItemDTO
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int RequiredQuantity { get; set; }
        [Required]
        public int IdCategory { get; set; }
        [Required]
        public int IdItem { get; set; }
        [Required]
        public int IdChecklist { get; set; }
    }
}