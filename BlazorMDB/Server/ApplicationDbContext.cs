using BlazorMDB.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorMDB.Server
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoviesPersons>().HasKey(x => new { x.MovieId, x.PersonId });
            modelBuilder.Entity<MoviesDirectors>().HasKey(x => new { x.MovieId, x.DirectorId });
            modelBuilder.Entity<MoviesGenres>().HasKey(x => new { x.MovieId, x.GenreId });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<MoviesPersons> MoviesPersons { get; set; }
        public DbSet<MoviesDirectors> MoviesDirectors { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }

    }
}
