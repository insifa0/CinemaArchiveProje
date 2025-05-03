using System.ComponentModel.DataAnnotations;

namespace CinemaArchiveProje.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        [Required]
        [StringLength(1000)]
        public string? Content { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } // Yorum puanı (1-5 arası)

        public DateTime DatePosted { get; set; }

        // Yorum yapan kullanıcı
        public int UserId { get; set; }             // Foreign key
        public User? User { get; set; }             // Navigation property
    }
}
