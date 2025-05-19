using CinemaArchiveProje.Models;
using System.Collections.Generic;

namespace CinemaArchiveProje.ViewModels
{
    public class HomePageViewModel
    {
        public Movie HeroMovie { get; set; }
        public Dictionary<string, List<Movie>> MoviesByGenre { get; set; }
        public string SearchString { get; set; }
        public int? SelectedGenreId { get; set; }
        public int? SelectedDirectorId { get; set; }
        public int? SelectedCountryId { get; set; }
    }
}
