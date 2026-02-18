using System.ComponentModel.DataAnnotations;

namespace Lakhani.Models
{
    public class Movies
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MovieName { get; set; }

        [Required]
        public TimeSpan ShowTime { get; set; }   // Only time

        [Required]
        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }

        public string Image { get; set; }
    }
}
