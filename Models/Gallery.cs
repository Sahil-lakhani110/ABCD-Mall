using System.ComponentModel.DataAnnotations;

namespace Lakhani.Models
{
    public class Gallery
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Image { get; set; }
    }
}
