using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using moje_filmy_api.Models;

namespace moje_filmy_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureMovies(modelBuilder.Entity<Movie>());
        }

        private void ConfigureMovies(EntityTypeBuilder<Movie> entity)
        {
            entity.ToTable("Movies");

            entity.Property(x => x.Title)
                .HasColumnName("Title")
                .HasColumnType("varchar(200)")
                .IsRequired();

            entity.Property(x => x.Director)
                .HasColumnName("Director")
                .HasColumnType("varchar(200)");

            entity.Property(x => x.Year)
                .HasColumnName("Year")
                .HasColumnType("int");

            entity.Property(x => x.Rate)
                .HasColumnName("Rate")
                .HasColumnType("int");

        }

        public DbSet<Movie> Movies { get; set; }
    }
}
