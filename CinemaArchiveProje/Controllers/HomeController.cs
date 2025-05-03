using Microsoft.AspNetCore.Mvc;
using CinemaArchiveProje.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CinemaArchiveProje.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var moviesQuery = _context.Movies
                .Include(m => m.Director)
                .Include(m => m.Genre)
                .Include(m => m.Country)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                moviesQuery = moviesQuery.Where(m => m.Title.Contains(searchString));
            }

            var latestMovies = await moviesQuery
                .OrderByDescending(m => m.ReleaseDate)
                .Take(5) // Arama yoksa son 5 film, arama varsa yine kısıtlama uygulanabilir ya da kaldırılabilir
                .ToListAsync();

            var latestReviews = await _context.Reviews
                .Include(r => r.Movie)
                .OrderByDescending(r => r.DatePosted)
                .Take(5)
                .ToListAsync();

            // İstatistik verileri
            ViewBag.MovieCount = await _context.Movies.CountAsync();
            ViewBag.DirectorCount = await _context.Directors.CountAsync();
            ViewBag.ActorCount = await _context.Actors.CountAsync();
            ViewBag.ReviewCount = await _context.Reviews.CountAsync();

            ViewBag.LatestReviews = latestReviews;

            return View(latestMovies);
        }



        public IActionResult Privacy()
        {
            return View();
        }
    }
}
