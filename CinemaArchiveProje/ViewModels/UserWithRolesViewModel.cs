using System.Collections.Generic;

namespace CinemaArchiveProje.Models.ViewModels
{
    public class UserWithRolesViewModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }

        public DateTime DateTime { get; set; }
    }
}