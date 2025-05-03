using Microsoft.AspNetCore.Mvc;
using CinemaArchiveProje.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaArchiveProje.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IActionResult> Index()
        {
            // Admin misin diye kontrol ediyorum burada
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            ViewBag.MovieCount = await _context.Movies.CountAsync();
            ViewBag.DirectorCount = await _context.Directors.CountAsync();
            ViewBag.ActorCount = await _context.Actors.CountAsync();
            ViewBag.ReviewCount = await _context.Reviews.CountAsync();
            ViewBag.UserCount = await _context.Users.CountAsync();
            ViewBag.GenresCount = await _context.Genres.CountAsync();
            ViewBag.CountriesCount = await _context.Countries.CountAsync();
            return View();
        }

        public async Task<IActionResult> UserList()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
    }
}
