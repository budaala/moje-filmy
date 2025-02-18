using Microsoft.EntityFrameworkCore;
using moje_filmy_api.Models;

namespace moje_filmy_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Movie> Movies { get; set; }
    }
}
