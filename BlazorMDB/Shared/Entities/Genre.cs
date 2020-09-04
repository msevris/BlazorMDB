﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BlazorMDB.Shared.Entities
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required")]
        public string Name { get; set; }

        public List<MoviesGenres> MoviesGenres { get; set; } = new List<MoviesGenres>();
    }
}
