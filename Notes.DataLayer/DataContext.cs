using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Notes.DataLayer.Models;

namespace Notes.DataLayer
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Note> Notes { get; set; } = null!;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Notes)
                .WithOne(n => n.Author)
                .HasForeignKey(n => n.AuthorId)
                .HasPrincipalKey(u => u.Id);
            modelBuilder.Entity<User>()
                .HasIndex(f => f.Email)
                .IsUnique();
            modelBuilder.Entity<Note>()
                .HasKey(n => n.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseMySQL(b => b.MigrationsAssembly("Notes.Main")); 
        }
    }
}
