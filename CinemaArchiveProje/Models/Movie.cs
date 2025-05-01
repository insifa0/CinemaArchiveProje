using System.ComponentModel.DataAnnotations;

namespace CinemaArchiveProje.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public int GenreId { get; set; }
        public Genre? Genre { get; set; }

        public int DirectorId { get; set; }
        public Director? Director { get; set; }

        public int CountryId { get; set; }
        public Country? Country { get; set; }

        public DateTime ReleaseDate { get; set; }

        public List<MovieActor>? MovieActors { get; set; } // Çoktan çoğa ilişki

        public List<Review>? Reviews { get; set; }
    }
}
