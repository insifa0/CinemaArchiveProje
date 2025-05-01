using System.ComponentModel.DataAnnotations;

namespace CinemaArchiveProje.Models
{
    public class Director
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Biography { get; set; }

        // Navigation Property
        public List<Movie>? Movies { get; set; }
    }
}
