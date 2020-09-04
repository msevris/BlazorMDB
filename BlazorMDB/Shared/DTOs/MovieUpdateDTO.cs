using BlazorMDB.Shared.Entities;
using System.Collections.Generic;

namespace BlazorMDB.Shared.DTOs
{
    public class MovieUpdateDTO
    {
        public Movie Movie { get; set; }
        public List<Person> Persons { get; set; }
        public List<Director> Directors { get; set; }
        public List<Genre> SelectedGenres { get; set; }
        public List<Genre> NotSelectedGenres { get; set; }
    }
}
