﻿using ItsCheck.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace ItsCheck.DTO
{
    public class ChecklistReviewDTO
    {
        [Required]
        public ReviewType Type { get; set; }
        public string? Observation { get; set; }
        public int? IdAmbulance { get; set; }
        [Required]
        public int IdChecklist { get; set; }
        [Required]
        public virtual required IList<CategoryReviewDTO> Categories { get; set; }
        public int? IdUser { get; set; }
    }

    public class CategoryReviewDTO
    {
        [Required]
        public required int Id { get; set; }
        [Required]
        public virtual required IEnumerable<ItemReviewDTO> Items { get; set; }
    }

    public class ItemReviewDTO
    {
        [Required]
        public required int Id { get; set; }
        [Required]
        public int AmountReplaced { get; set; }
    }
}