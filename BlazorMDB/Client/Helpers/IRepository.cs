using BlazorMDB.Shared.Entities;
using System.Collections.Generic;

namespace BlazorMDB.Client.Helpers
{
    public interface IRepository
    {
        List<Movie> GetMovies();
    }
}
