using System.ComponentModel.DataAnnotations;

namespace Lakhani.Models
{
    public class Shops
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Shop name is required")]
        [StringLength(100, ErrorMessage = "Shop name cannot exceed 100 characters")]
        public string? ShopName { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        public string? Category { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}
