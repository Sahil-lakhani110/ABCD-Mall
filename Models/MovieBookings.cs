using System.ComponentModel.DataAnnotations;

namespace Lakhani.Models
{
    public class MovieBookings
    {
        [Key]
        public int Id { get; set; }

        public int MovieId { get; set; }
        public Movies? Movie { get; set; }

        public string UserName { get; set; } = string.Empty;

        public int SeatsBooked { get; set; }

        public DateTime BookingDate { get; set; }

        public decimal Amount { get; set; }
    }
}
