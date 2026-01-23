using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectBackend.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; } // zamiast name, spójnie z TMDB

        public string Overview { get; set; }

        public bool Adult { get; set; }

        [NotMapped]
        public int[] GenreIds { get; set; } // tymczasowe do mapowania gatunków

        public DateTime ReleaseDate { get; set; }

        public float VoteAverage { get; set; }

        public string PosterPath { get; set; }

        public string BackdropPath { get; set; } // dodałem, bo TMDB zwraca tło

        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    }
}
