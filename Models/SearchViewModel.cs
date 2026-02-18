namespace Lakhani.Models
{
    public class SearchViewModel
    {
        public string Query { get; set; }

        // List of movies that match the search
        public List<Movies> Movies { get; set; } = new List<Movies>();

        // List of food courts that match the search
        public List<FoodCourts> FoodCourts { get; set; } = new List<FoodCourts>();

        // List of shops that match the search
        public List<Shops> Shops { get; set; } = new List<Shops>();
    }
}
