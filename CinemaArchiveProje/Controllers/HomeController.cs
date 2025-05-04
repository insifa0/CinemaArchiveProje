using Microsoft.AspNetCore.Mvc;
using CinemaArchiveProje.Models;
using CinemaArchiveProje.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaArchiveProje.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IActionResult> Index(string searchString, int? genreId, int? directorId, int? countryId)
        {
            var query = _context.Movies
                .Include(m => m.Director)
                .Include(m => m.Genre)
                .Include(m => m.Country)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(m => m.Title.Contains(searchString));
            }

            if (genreId.HasValue && genreId.Value > 0)
                query = query.Where(m => m.GenreId == genreId.Value);

            if (directorId.HasValue && directorId.Value > 0)
                query = query.Where(m => m.DirectorId == directorId.Value);

            if (countryId.HasValue && countryId.Value > 0)
                query = query.Where(m => m.CountryId == countryId.Value);

            var movies = await query.ToListAsync();

            ViewBag.Genres = new SelectList(await _context.Genres.ToListAsync(), "Id", "Name", genreId);
            ViewBag.Directors = new SelectList(await _context.Directors.ToListAsync(), "Id", "Name", directorId);
            ViewBag.Countries = new SelectList(await _context.Countries.ToListAsync(), "Id", "Name", countryId);
            ViewBag.SearchString = searchString;

            ViewBag.LatestReviews = await _context.Reviews
                .OrderByDescending(r => r.DatePosted)
                .Take(5)
                .ToListAsync();
            ViewBag.MovieCount = await _context.Movies.CountAsync();
            ViewBag.DirectorCount = await _context.Directors.CountAsync();
            ViewBag.ActorCount = await _context.Actors.CountAsync();
            ViewBag.ReviewCount = await _context.Reviews.CountAsync();

            return View(movies);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error404()
        {
            return View("NotFound");
        }

    }
}
