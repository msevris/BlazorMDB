using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorMDB.Shared.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime? ReleaseDate { get; set; }
        public string Trailer { get; set; }
        public int ReleaseYear { get; set; }
        public string Duration { get; set; }
        public string ParentalGuide { get; set; }
        public string Poster { get; set; }
        public string Summary { get; set; }
        public int Gross { get; set; }
        public string TitleBrief
        {
            get
            {
                if (string.IsNullOrEmpty(Title)) { return null; }
                if (Title.Length > 60) { return Title.Substring(0, 60) + "..."; }
                else { return Title; }
            }
        }
        public string SummaryBrief
        {
            get
            {
                if (string.IsNullOrEmpty(Summary)) { return null; }
                if (Summary.Length > 150) { return Summary.Substring(0, 150) + "..."; }
                else { return Summary; }
            }
        }

        public List<MoviesGenres> MoviesGenres { get; set; } = new List<MoviesGenres>();
        public List<MoviesPersons> MoviesPersons { get; set; } = new List<MoviesPersons>();
        public List<MoviesDirectors> MoviesDirectors { get; set; } = new List<MoviesDirectors>();
    }
}

