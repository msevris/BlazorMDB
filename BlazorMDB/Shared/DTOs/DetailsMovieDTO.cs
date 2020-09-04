using BlazorMDB.Shared.Entities;
using System.Collections.Generic;

namespace BlazorMDB.Shared.DTOs
{
    public class DetailsMovieDTO
    {
        public Movie Movie { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Person> Persons { get; set; }
        public List<Director> Directors { get; set; }
    }
}
