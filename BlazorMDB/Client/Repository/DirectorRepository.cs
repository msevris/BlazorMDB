using BlazorMDB.Client.Helpers;
using BlazorMDB.Shared.DTOs;
using BlazorMDB.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMDB.Client.Repository
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly IHttpService httpService;
        private string url = "api/director";
        public DirectorRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task CreateDirector(Director director)
        {
            var response = await httpService.Post(url, director);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task DeleteDirector(int id)
        {
            var response = await httpService.Delete($"{url}/{id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task<Director> GetDirectorById(int id)
        {
            return await httpService.GetHelper<Director>($"{url}/{id}");
        }

        public async Task<List<Director>> GetDirectorByName(string name)
        {
            var response = await httpService.Get<List<Director>>($"{url}/search/{name}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<PaginatedResponse<List<Director>>> GetDirectors(PaginationDTO paginationDTO)
        {
            return await httpService.GetHelper<List<Director>>(url, paginationDTO);
        }

        public async Task UpdateDirector(Director director)
        {
            var response = await httpService.Put(url, director);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
