using System.ComponentModel.DataAnnotations;

namespace CinemaArchiveProje.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        // Navigation Property
        public List<Movie>? Movies { get; set; }
    }
}
