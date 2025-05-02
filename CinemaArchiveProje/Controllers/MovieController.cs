using CinemaArchiveProje.Data;
using CinemaArchiveProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CinemaArchiveProje.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies
                .Include(m => m.Director)
                .Include(m => m.Genre)
                .Include(m => m.Country)
                .ToListAsync();
            return View(movies);
        }

        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, IFormFile posterFile)
        {
            if (ModelState.IsValid)
            {
                if (posterFile != null && posterFile.Length > 0)
                {
                    // Dosya adını al
                    var fileName = Path.GetFileName(posterFile.FileName);

                    // Dosyanın kaydedileceği yolu belirleyin
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    // Dosya kaydetme işlemi
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await posterFile.CopyToAsync(stream);
                    }

                    // Movie modeline dosyanın yolunu kaydet
                    movie.PosterUrl = "/images/" + fileName;
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa ViewData yeniden doldurulmalı
            ViewData["Directors"] = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
            ViewData["Genres"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            ViewData["Countries"] = new SelectList(_context.Countries, "Id", "Name", movie.CountryId);

            return View(movie);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Directors"] = new SelectList(_context.Directors, "Id", "Name");
            ViewData["Genres"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["Countries"] = new SelectList(_context.Countries, "Id", "Name");

            return View();
        }


        // Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            // Dropdown'ları doldur
            ViewBag.Directors = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            ViewBag.Countries = new SelectList(_context.Countries, "Id", "Name", movie.CountryId);

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Form hatalıysa dropdown'ları tekrar doldur
            ViewBag.Directors = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            ViewBag.Countries = new SelectList(_context.Countries, "Id", "Name", movie.CountryId);

            return View(movie);
        }


        // GET Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.Director)
                .Include(m => m.Genre)
                .Include(m => m.Country) // Eğer tüm ilişkili verileri göstermek istiyorsanız
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        // POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Yönlendirme işlemi
        }

        //DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.Director)
                .Include(m => m.Genre)
                .Include(m => m.Country)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            return View(movie);
        }




    }
}
