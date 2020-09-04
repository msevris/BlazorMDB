using BlazorMDB.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMDB.Client.Repository
{
    public interface IGenreRepository
    {
        Task CreateGenre(Genre genre);
        Task<Genre> GetGenre(int Id);
        Task<List<Genre>> GetGenres();
        Task UpdateGenre(Genre genre);
        Task DeleteGenre(int Id);
    }
}
