using BlazorMDB.Shared.DTOs;
using BlazorMDB.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMDB.Client.Repository
{
    public interface IDirectorRepository
    {
        Task CreateDirector(Director director);
        Task DeleteDirector(int id);
        Task<Director> GetDirectorById(int id);
        Task<List<Director>> GetDirectorByName(string name);
        Task<PaginatedResponse<List<Director>>> GetDirectors(PaginationDTO paginationDTO);
        Task UpdateDirector(Director director);
    }
}
