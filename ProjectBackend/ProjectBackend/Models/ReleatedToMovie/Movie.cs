using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectBackend.Models.ReleatedToMovie;

public class Movie
{
    public int Id { get; set; }
    public int TmdbId { get; set; }
    public string Title { get; set; } // zamiast name, spójnie z TMDB
    public string Overview { get; set; }
    public DateTime ReleaseDate { get; set; }
    public float VoteAverage { get; set; }
    public string? PosterPath { get; set; }
    public string? BackdropPath { get; set; } 
    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    public ICollection<MoviePeopleRole> MoviePeopleRoles { get; set; } = new List<MoviePeopleRole>();
    public ICollection<MovieCompany> MovieCompanies { get; set; } = new List<MovieCompany>();

}
