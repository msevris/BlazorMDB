using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorMDB.Shared.Entities
{
   public class MoviesDirectors
    {
        public int DirectorId { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public Director Director { get; set; }
       
    }
}
