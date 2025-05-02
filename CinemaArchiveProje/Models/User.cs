using System.ComponentModel.DataAnnotations;

namespace CinemaArchiveProje.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Username { get; set; }

        [Required]
        [StringLength(256)]
        public string? Password { get; set; } // Şifre (gizli tutulacak)

        [StringLength(100)]
        public string? Email { get; set; }

        [Required]
        public string Role { get; set; } = "User"; // veya "Admin"

        public DateTime DateJoined { get; set; } // Kullanıcının katılma tarihi

        // Kullanıcının yorumları (1 kullanıcının birden fazla yorumu olabilir)
        public List<Review>? Reviews { get; set; }

    }
}
