using Microsoft.EntityFrameworkCore;

namespace Lakhani.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Shops> Shops { get; set; }
        public DbSet<FoodCourts> FoodCourts { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Gallery> Gallerys { get; set; }
        public DbSet<Signup> Signup { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<MovieBookings> MovieBookings { get; set; }

    }
}
