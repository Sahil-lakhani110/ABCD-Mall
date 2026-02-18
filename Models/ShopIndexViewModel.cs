namespace Lakhani.Models
{
    public class ShopIndexViewModel
    {
        public IEnumerable<Shops> Shops { get; set; }
        public IEnumerable<Gallery> Galleries { get; set; }
        public Feedback Feedback { get; set; }
    }
}
