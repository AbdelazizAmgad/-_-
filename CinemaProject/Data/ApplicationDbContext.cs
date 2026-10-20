using Microsoft.EntityFrameworkCore;
using CinemaProject.Models;
using Microsoft.Identity.Client;
namespace CinemaProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Cinema> Cinemas { get; set; }
            public DbSet<Actor> Actors { get; set; }
            public DbSet<Movie> Movies { get; set; }
            public DbSet<MovieSubImage> MovieSubImages { get; set; }
            public DbSet<Booking> Bookings { get; set; }

    }
}
