using Microsoft.AspNetCore.Mvc;
using CinemaArchiveProje.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CinemaArchiveProje.Controllers
{

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.MovieCount = await _context.Movies.CountAsync();
            ViewBag.DirectorCount = await _context.Directors.CountAsync();
            ViewBag.ActorCount = await _context.Actors.CountAsync();
            ViewBag.ReviewCount = await _context.Reviews.CountAsync();
            ViewBag.UserCount = await _context.Users.CountAsync();
            ViewBag.GenresCount = await _context.Genres.CountAsync();

            return View();
        }


        // User Listesi
        public async Task<IActionResult> UserList()
        {
            // Kullanıcıları al
            var users = await _context.Users.ToListAsync();
            return View(users);  // View'da kullanıcılar listelenecek
        }
    }
}
