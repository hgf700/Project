using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using ProjectBackend.Models;

namespace ProjectBackend.DB;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }

    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistValue> PlaylistValues{ get; set; }
    public DbSet<LikedMaterial> LikedMaterials { get; set; }
    public DbSet<Friend> Friends { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MovieGenre>()
            .HasKey(mg => new { mg.MovieId, mg.GenreId });

        modelBuilder.Entity<LikedMaterial>()
            .HasKey(lm => new { lm.UserId, lm.MovieId });

        modelBuilder.Entity<PlaylistValue>()
            .HasKey(pv => new { pv.PlaylistId, pv.MovieId });

        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId);

        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Genre)
            .WithMany(g => g.MovieGenres)
            .HasForeignKey(mg => mg.GenreId);

        modelBuilder.Entity<LikedMaterial>()
            .HasOne(lm => lm.User)
            .WithMany()
            .HasForeignKey(lm => lm.UserId);

        modelBuilder.Entity<LikedMaterial>()
            .HasOne(lm => lm.Movie)
            .WithMany()
            .HasForeignKey(lm => lm.MovieId);

        modelBuilder.Entity<PlaylistValue>()
            .HasOne(pv => pv.Playlist)
            .WithMany()
            .HasForeignKey(pv => pv.PlaylistId);

        modelBuilder.Entity<PlaylistValue>()
            .HasOne(pv => pv.Movie)
            .WithMany()
            .HasForeignKey(pv => pv.MovieId);

        modelBuilder.Entity<Friend>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Friend>()
            .HasOne(f => f.FriendUser)
            .WithMany()
            .HasForeignKey(f => f.FriendUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}