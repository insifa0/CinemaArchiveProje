using CinemaArchiveProje.Data;
using CinemaArchiveProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaArchiveProje.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index - Yorumları listele
        public async Task<IActionResult> Index(int? movieId)
        {
            List<Review> reviews;

            if (movieId == null)
            {
                // Tüm yorumları getir
                reviews = await _context.Reviews
                    .Include(r => r.Movie)
                    .OrderByDescending(r => r.DatePosted)
                    .ToListAsync();

                ViewData["MovieTitle"] = null;
                ViewData["MovieId"] = null;
            }
            else
            {
                // Belirli filme ait yorumları getir
                var movie = await _context.Movies
                    .Include(m => m.Reviews)
                    .FirstOrDefaultAsync(m => m.Id == movieId);

                if (movie == null)
                {
                    return NotFound();
                }

                ViewData["MovieTitle"] = movie.Title;
                ViewData["MovieId"] = movieId;

                reviews = movie.Reviews
                    .OrderByDescending(r => r.DatePosted)
                    .ToList();
            }

            return View(reviews);
        }


        // Create - Yeni yorum ekle
        public IActionResult Create(int movieId)
        {
            ViewBag.MovieId = movieId;
            return View();
        }

        // Create (POST) - Yeni yorum ekleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int movieId, [Bind("MovieId,Content,Rating")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.MovieId = movieId;
                review.DatePosted = DateTime.Now;
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { movieId = movieId });
            }
            ViewBag.MovieId = movieId;
            return View(review);
        }


        // Edit - Yorum düzenle
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // Edit (POST) - Yorum düzenleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,Content,Rating,DatePosted")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Mevcut review verisini çekiyoruz
                    var existingReview = await _context.Reviews.FindAsync(id);
                    if (existingReview == null)
                        return NotFound();

                    // Sadece değiştirilebilir alanları güncelliyoruz
                    existingReview.Content = review.Content;
                    existingReview.Rating = review.Rating;
                    existingReview.DatePosted = DateTime.Now; // İstersen güncellenmesin

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index), new { movieId = review.MovieId });
            }
            return View(review);
        }


        // Delete - Yorum sil
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // Delete (POST) - Yorum silme işlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            int? movieId = null;

            if (review != null)
            {
                movieId = review.MovieId;
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
