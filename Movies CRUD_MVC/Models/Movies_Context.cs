using Microsoft.EntityFrameworkCore;
using Movies_CRUD_MVC.Entites;

namespace Movies_CRUD_MVC.Models
{
    public class Movies_Context:DbContext
    {

        public Movies_Context(DbContextOptions<Movies_Context> options) : base(options) 
        { 
        }

        public DbSet<genre> Genres { get; set; }
        public DbSet <Movie> Movies { get; set; }

    }
}
