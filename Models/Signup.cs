using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lakhani.Models
{
    public class Signup
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Username Is Required")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Username Should be Between 3 to 15")]
        [RegularExpression(@"^[a-zA-Z ]{3,15}$", ErrorMessage = "Username in Alphabet")]
        public string Username { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email is not Valid")]
        public string Email { get; set; }
        [Required]
        //[StringLength(12, MinimumLength = 4, ErrorMessage = "Password Should be Between 4 to 12")]
        public string Password { get; set; }
        [Required]
        [NotMapped]
        [Compare("Password", ErrorMessage = "Password isn't Match!")]
        public string RetypePassword { get; set; }
        public string Role { get; set; } = "User";
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
