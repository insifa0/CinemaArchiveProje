﻿using System.ComponentModel.DataAnnotations.Schema;
namespace CinemaArchiveProje.Models
{
    public class MovieActor
    {
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        public int ActorId { get; set; }
        public Actor? Actor { get; set; }
    }
}
