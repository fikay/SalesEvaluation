using Microsoft.EntityFrameworkCore;

namespace SalesEvaluation.Backend.Models
{
    public class AppContext : DbContext
    {

        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
        }
        public DbSet<FilesDetails> Files { get; set; } = null!;   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilesDetails>()
                .HasKey(f => f.Id);
            modelBuilder.Entity<FilesDetails>()
                .Property(f => f.FileName)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder.Entity<FilesDetails>()
                .Property(f => f.Content)
                .IsRequired();
        }
    }
}
