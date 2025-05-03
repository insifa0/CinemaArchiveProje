using CinemaArchiveProje.Data;
using CinemaArchiveProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace CinemaArchiveProje.Controllers
{
    public class MovieController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context) : base(context)
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
                    movie.PosterUrl = movie.PosterUrl ?? "/images/default.jpg";
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
        // GET: Movie/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            ViewBag.Directors = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            ViewBag.Countries = new SelectList(_context.Countries, "Id", "Name", movie.CountryId);

            return View(movie);
        }

        // POST: Movie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, IFormFile posterFile)
        {
            if (id != movie.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existingMovie = await _context.Movies.FindAsync(id);
                if (existingMovie == null) return NotFound();

                existingMovie.Title = movie.Title;
                existingMovie.Description = movie.Description;
                existingMovie.ReleaseDate = movie.ReleaseDate;
                existingMovie.DirectorId = movie.DirectorId;
                existingMovie.GenreId = movie.GenreId;
                existingMovie.CountryId = movie.CountryId;
                existingMovie.TrailerUrl = movie.TrailerUrl; // 👈 Yeni alan eklendi

                if (posterFile != null && posterFile.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(posterFile.FileName);
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await posterFile.CopyToAsync(stream);
                    }

                    existingMovie.PosterUrl = "/images/" + fileName;
                }

                _context.Update(existingMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

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
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            // Review'ları ayrı olarak çekiyoruz
            var reviews = await _context.Reviews
                .Where(r => r.MovieId == movie.Id)
                .OrderByDescending(r => r.DatePosted)
                .ToListAsync();

            ViewBag.Reviews = reviews;
            ViewBag.IsAdmin = IsAdmin();

            return View(movie);
        }


        //ADD review
        [HttpPost]
        public async Task<IActionResult> AddReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", new { id = review.MovieId });
            }

            var username = HttpContext.Session.GetString("Username");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                review.UserId = user.Id;
                review.DatePosted = DateTime.Now;
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = review.MovieId });
        }

    }
}
