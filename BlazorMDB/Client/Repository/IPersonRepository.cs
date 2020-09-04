using BlazorMDB.Shared.DTOs;
using BlazorMDB.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMDB.Client.Repository
{
    public interface IPersonRepository
    {
        Task CreatePerson(Person person);
        Task DeletePerson(int id);
        Task<PaginatedResponse<List<Person>>> GetPersons(PaginationDTO paginationDTO);
        Task<List<Person>> GetPersonByName(string name);
        Task<Person> GetPersonById(int id);
        Task UpdatePerson(Person person);
    }
}
