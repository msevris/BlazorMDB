using BlazorMDB.Shared.Entities;
using System;
using System.Collections.Generic;

namespace BlazorMDB.Client.Helpers
{
    public class RepositoryInMemory : IRepository
    {
        public List<Movie> GetMovies()
        {
            return new List<Movie>()
            {
                new Movie()
                {
                    Title = "The Shawshank Redemption",
                    ReleaseDate = new DateTime(1994,10,14),
                    Poster = "https://m.media-amazon.com/images/M/MV5BMDFkYTc0MGEtZmNhMC00ZDIzLWFmNTEtODM1ZmRlYWMwMWFmXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_.jpg",
                    ReleaseYear = 1994,
                    ParentalGuide = "R",
                    Duration = "2h 22min",
                    Gross = 28699976,
                    Summary = "Two imprisoned men bond over a number of years, finding solace and eventual " +
                              "redemption through acts of common decency.",
                    Trailer = "https://www.youtube.com/watch?v=6hB3S9bIaco"
                },
                new Movie()
                {
                    Title = "The Godfather",
                    ReleaseDate = new DateTime(1972,4,24),
                    Poster = "https://m.media-amazon.com/images/M/MV5BM2MyNjYxNmUtYTAwNi00MTYxLWJmNWYtYzZlODY3ZTk3OTFlXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_SY1000_CR0,0,704,1000_AL_.jpg",
                    ReleaseYear = 1972,
                    ParentalGuide = "R",
                    Duration = "2h 55min",
                    Gross = 28699976,
                    Summary = "The aging patriarch of an organized crime dynasty transfers control of his " +
                              "clandestine empire to his reluctant son.",
                    Trailer = "https://www.youtube.com/watch?v=sY1S34973zA"
                }
            };
        }
    }
}
