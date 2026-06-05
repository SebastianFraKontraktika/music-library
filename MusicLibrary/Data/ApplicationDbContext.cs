using Microsoft.EntityFrameworkCore;
using MusicLibrary.Models;

namespace MusicLibrary.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Music> Musics { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<MusicGenre> MusicGenres { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
         options.UseSqlite("Data Source=musicLib.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        modelBuilder.Entity<MusicGenre>()
            .HasKey(mg => new { mg.MusicId, mg.GenreId });
    }
}