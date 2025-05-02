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

        public async Task<IActionResult> Index()
        {
            var latestMovies = await _context.Movies
                .OrderByDescending(m => m.ReleaseDate)
                .Take(5)
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

            return View(latestMovies); // Model olarak son filmler gönderiliyor
        }


        public IActionResult Privacy()
        {
            return View();
        }
    }
}
