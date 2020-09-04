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
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;
        private readonly IMapper mapper;
        private string containerName = "movies";

        public MoviesController(ApplicationDbContext context,
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Movie>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Movies.AsQueryable();
            await HttpContext.InsertPaginationParametersInResponse(queryable, paginationDTO.RecordsPerPage);
            return await queryable.Paginate(paginationDTO).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetailsMovieDTO>> Get(int id)
        {
           var movie = await context.Movies.Where(x => x.Id == id)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesPersons).ThenInclude(x => x.Person)
                .Include(x => x.MoviesDirectors).ThenInclude(x => x.Director)
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                return NotFound();
            }

            movie.MoviesPersons = movie.MoviesPersons.OrderBy(x => x.Order).ToList();
            //movie.MoviesDirectors = movie.MoviesDirectors.OrderBy(x => x.Order).ToList();

            var model = new DetailsMovieDTO();
            model.Movie = movie;
            model.Genres = movie.MoviesGenres.Select(x => x.Genre).OrderBy(x => x.Name).ToList();
            model.Persons = movie.MoviesPersons.Select(x =>
                new Person
                {
                    Name = x.Person.Name,
                    Picture = x.Person.Picture,
                    Character = x.Character,
                    Id = x.PersonId
                }).ToList();
            
            model.Directors = movie.MoviesDirectors.Select(x =>
               new Director
               {
                   Name = x.Director.Name,
                   Picture = x.Director.Picture,
                   Id = x.DirectorId
               }).ToList();

            return model;
        }

        [HttpPost("filter")]
        public async Task<ActionResult<List<Movie>>> Filter(FilterMoviesDTO filterMoviesDTO)
        {
            var moviesQueryable = context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterMoviesDTO.Title))
            {
                moviesQueryable = moviesQueryable.Where(x => x.Title.Contains(filterMoviesDTO.Title));
            }
            if (filterMoviesDTO.GenreId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.MoviesGenres.Select(y => y.GenreId)
                    .Contains(filterMoviesDTO.GenreId));
            }

            await HttpContext.InsertPaginationParametersInResponse(moviesQueryable, filterMoviesDTO.RecordsPerPage);
            var movies = await moviesQueryable.Paginate(filterMoviesDTO.Pagination).ToListAsync();

            return movies;
        }

        [HttpGet("update/{id}")]
        public async Task<ActionResult<MovieUpdateDTO>> PutGet(int id)
        {
           var movieActionResult = await Get(id);
            if (movieActionResult.Result is NotFoundResult)
            {
                return NotFound();
            }
            var movieDetailDTO = movieActionResult.Value;
            var selectedGenresIds = movieDetailDTO.Genres.Select(x => x.Id).ToList();
            var notSelectedGenres = await context.Genres
                .Where(x => !selectedGenresIds.Contains(x.Id))
                .ToListAsync();

            var model = new MovieUpdateDTO();
            model.Movie = movieDetailDTO.Movie;
            model.SelectedGenres = movieDetailDTO.Genres;
            model.NotSelectedGenres = notSelectedGenres;
            model.Persons = movieDetailDTO.Persons;
            model.Directors = movieDetailDTO.Directors;

            return model;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Movie movie)
        {
            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var poster = Convert.FromBase64String(movie.Poster);
                movie.Poster = await fileStorageService.SaveFile(poster, "jpg", containerName);
            }

            if (movie.MoviesPersons != null)
            {
                for (int i = 0; i < movie.MoviesPersons.Count; i++)
                {
                    movie.MoviesPersons[i].Order = i + 1;
                }
            }
            if (movie.MoviesDirectors != null)
            {
                movie.MoviesDirectors.Count();
            }
            //if (movie.MoviesDirectors != null)
            //{
            //    for (int i = 0; i < movie.MoviesDirectors.Count; i++)
            //    {
            //        movie.MoviesDirectors[i].Order = i + 1;
            //    }
            //}

            context.Add(movie);
            await context.SaveChangesAsync();
            return movie.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Movie movie)
        {
            var movieDB = await context.Movies.FirstOrDefaultAsync(x => x.Id == movie.Id);
            if (movieDB == null)
            {
                return NotFound();
            }
            movieDB = mapper.Map(movie, movieDB);

            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var moviePoster = Convert.FromBase64String(movie.Poster);
                movieDB.Poster = await fileStorageService.EditFile(moviePoster,
                    "jpg", containerName, movieDB.Poster);
            }
            await context.Database.ExecuteSqlInterpolatedAsync($"delete from MoviesDirectors where MovieId = {movie.Id};delete from MoviesPersons where MovieId = {movie.Id}; delete from MoviesGenres where MovieId = {movie.Id}");

            if (movie.MoviesPersons != null)
            {
                for (int i = 0; i < movie.MoviesPersons.Count; i++)
                {
                    movie.MoviesPersons[i].Order = i + 1;
                }
            }
            //if (movie.MoviesDirectors != null)
            //{
            //    for (int i = 0; i < movie.MoviesDirectors.Count; i++)
            //    {
            //        movie.MoviesDirectors[i].Order = i + 1;
            //    }
            //}

            movieDB.MoviesPersons = movie.MoviesPersons;
            movieDB.MoviesDirectors = movie.MoviesDirectors;
            movieDB.MoviesGenres = movie.MoviesGenres;

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            context.Remove(movie);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
