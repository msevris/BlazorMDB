using AutoMapper;
using BlazorMDB.Server.Helpers;
using BlazorMDB.Shared.DTOs;
using BlazorMDB.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMDB.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;
        private readonly IMapper mapper;

        public DirectorController(ApplicationDbContext context,
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Director>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Directors.AsQueryable();
            await HttpContext.InsertPaginationParametersInResponse(queryable, paginationDTO.RecordsPerPage);
            return await queryable.Paginate(paginationDTO).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Director>> Get(int id)
        {
            var director = await context.Directors.FirstOrDefaultAsync(x => x.Id == id);
            if (director == null) { return NotFound(); }
            return director;
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Director>>> FilterByName(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) { return new List<Director>(); }
            return await context.Directors.Where(x => x.Name.Contains(searchText))
                .Take(5)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Director director)
        {
            if (!string.IsNullOrWhiteSpace(director.Picture))
            {
                var directorPicture = Convert.FromBase64String(director.Picture);
                director.Picture = await fileStorageService.SaveFile(directorPicture, "jpg", "director");
            }

            context.Add(director);
            await context.SaveChangesAsync();
            return director.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Director director)
        {
            var directorDB = await context.Directors.FirstOrDefaultAsync(x => x.Id == director.Id);
            if (directorDB == null) { return NotFound(); }
            directorDB = mapper.Map(director, directorDB);
            if (!string.IsNullOrWhiteSpace(director.Picture))
            {
                var directorPicture = Convert.FromBase64String(director.Picture);
                directorDB.Picture = await fileStorageService.EditFile(directorPicture,
                    "jpg", "director", directorDB.Picture);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var director = await context.Directors.FirstOrDefaultAsync(x => x.Id == id);
            if (director == null)
            {
                return NotFound();
            }

            context.Remove(director);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
