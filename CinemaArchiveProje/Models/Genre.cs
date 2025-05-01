using System.ComponentModel.DataAnnotations;

namespace CinemaArchiveProje.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        // Navigation Property
        public List<Movie>? Movies { get; set; }
    }
}

