using Lakhani.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lakhani.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext context;

        public SearchController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                // Return empty view if no search term
                return View(new SearchViewModel());
            }

            // Search Movies
            var movies = await this.context.Movies
                .Where(m => m.MovieName.Contains(query))
                .ToListAsync();

            // Search Food Courts
            var foodCourts = await this.context.FoodCourts
                .Where(f => f.CounterName.Contains(query))
                .ToListAsync();

            // Search Shops
            var shops = await this.context.Shops
                .Where(s => s.ShopName.Contains(query))
                .ToListAsync();

            var model = new SearchViewModel
            {
                Query = query,
                Movies = movies,
                FoodCourts = foodCourts,
                Shops = shops
            };

            return View(model);
        }
    }
}
