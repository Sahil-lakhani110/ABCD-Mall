using System.ComponentModel.DataAnnotations;

namespace Lakhani.Models
{
    public class FoodCourts
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Counter name is required")]
        [StringLength(100)]
        public string? CounterName { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        [StringLength(100)]
        public string? ItemName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 100000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public string? ImagePath { get; set; }
    }
}
