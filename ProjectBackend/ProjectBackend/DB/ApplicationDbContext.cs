    using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using ProjectBackend.Models.RelatedToRecommendation;
using ProjectBackend.Models.ReleatedToMovie;
using ProjectBackend.Models.ReleatedToPlaylist;
using ProjectBackend.Models.ReleatedToSocial;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProjectBackend.DB;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<RecomendTag> RecomendTags { get; set; }
    public DbSet<RecomendTagMovie> RecomendTagMovies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistComment> PlaylistComments { get; set; }
    public DbSet<PlaylistValue> PlaylistValues{ get; set; }
    public DbSet<PlaylistMember> PlaylistMembers { get; set; }
    public DbSet<PlaylistLike> PlaylistLikes { get; set; }
    public DbSet<UserMediaStatus> UserMediaStatuses { get; set; }
    public DbSet<UserComment> UserComments { get; set; }
    public DbSet<UserFollow> UserFollows { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<PrefferedGenre> PrefferedGenres { get; set; }
    public DbSet<MovieUserPreference> MovieUserPreferences { get; set; }
    public DbSet<PeopleRole> PeopleRoles { get; set; }
    public DbSet<MoviePeopleRole> MoviePeopleRoles { get; set; }
    public DbSet<MovieCompany> MovieCompanies { get; set; }
    public DbSet<Company> Companies { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CommentBase>()
            .UseTphMappingStrategy();

        modelBuilder.Entity<MoviePeopleRole>()
            .HasKey(mg => new { mg.MovieId, mg.PeopleRolesId });

        modelBuilder.Entity<MoviePeopleRole>()
            .HasOne(mg => mg.Movie)
            .WithMany(g => g.MoviePeopleRole)
            .HasForeignKey(mg => mg.MovieId);

        modelBuilder.Entity<MoviePeopleRole>()
            .HasOne(mg => mg.PeopleRole)
            .WithMany(g => g.MoviePeopleRole)
            .HasForeignKey(mg => mg.PeopleRolesId);

        modelBuilder.Entity<MovieGenre>()
            .HasKey(mg => new { mg.MovieId, mg.GenreId });

        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenre)
            .HasForeignKey(mg => mg.MovieId);

        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Genre)
            .WithMany(g => g.MovieGenre)
            .HasForeignKey(mg => mg.GenreId);

        modelBuilder.Entity<RecomendTagMovie>()
            .HasKey(mg => new { mg.MovieId, mg.RecomendTagId });

        modelBuilder.Entity<RecomendTagMovie>()
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.RecomendTagMovie)
            .HasForeignKey(mg => mg.MovieId);

        modelBuilder.Entity<RecomendTagMovie>()
            .HasOne(mg => mg.RecomendTag)
            .WithMany(g => g.RecomendTagMovie)
            .HasForeignKey(mg => mg.RecomendTagId);

        modelBuilder.Entity<MovieCompany>()
            .HasKey(mg => new { mg.MovieId, mg.CompanyId });

        modelBuilder.Entity<MovieCompany>()
            .HasOne(mg => mg.Movie)
            .WithMany(g => g.MovieCompanie)
            .HasForeignKey(mg => mg.MovieId);

        modelBuilder.Entity<MovieCompany>()
            .HasOne(mg => mg.Company)
            .WithMany(g => g.MovieCompanie)
            .HasForeignKey(mg => mg.CompanyId);

        var dictConverter = new ValueConverter<Dictionary<int, double>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
            v => JsonSerializer.Deserialize<Dictionary<int, double>>(v, (JsonSerializerOptions)null)
        );

        modelBuilder.Entity<MovieUserPreference>()
            .HasKey(p => p.UserId);

        modelBuilder.Entity<MovieUserPreference>()
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<MovieUserPreference>(p => p.UserId);

        modelBuilder.Entity<MovieUserPreference>()
            .Property(p => p.GenreWeights)
            .HasConversion(dictConverter);

        modelBuilder.Entity<MovieUserPreference>()
            .Property(p => p.YearBuckets)
            .HasConversion(dictConverter);

        modelBuilder.Entity<MovieUserPreference>()
            .Property(p => p.TmdbRatingBuckets)
            .HasConversion(dictConverter);

        modelBuilder.Entity<MovieUserPreference>()
            .Property(p => p.ActorWeights)
            .HasConversion(dictConverter);

        modelBuilder.Entity<UserMediaStatus>()
            .HasIndex(um => new { um.UserId, um.MovieId })
            .IsUnique();

        modelBuilder.Entity<UserMediaStatus>()
            .HasOne(lm => lm.User)
            .WithMany()
            .HasForeignKey(lm => lm.UserId);

        modelBuilder.Entity<UserMediaStatus>()
            .HasOne(lm => lm.Movie)
            .WithMany()
            .HasForeignKey(lm => lm.MovieId);

        modelBuilder.Entity<UserComment>()
            .HasOne(c => c.User)
            .WithMany(u => u.UserCommentsWritten)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserComment>()
            .HasOne(c => c.TargetUser)
            .WithMany(u => u.UserCommentsReceived)
            .HasForeignKey(c => c.TargetUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserFollow>()
            .HasKey(uf => new { uf.UserId, uf.TargetUserId });

        modelBuilder.Entity<UserFollow>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.Following) // zakładamy ICollection<UserFollow> Following w ApplicationUser
            .HasForeignKey(uf => uf.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserFollow>()
            .HasOne(uf => uf.TargetUser)
            .WithMany(u => u.Followers) // zakładamy ICollection<UserFollow> Followers w ApplicationUser
            .HasForeignKey(uf => uf.TargetUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PlaylistLike>()
            .HasKey(pl => new { pl.PlaylistId, pl.UserId }); 

        modelBuilder.Entity<PlaylistLike>()
            .HasOne(pl => pl.Playlist)
            .WithMany(p => p.Likes) // playlista ma kolekcję lajków
            .HasForeignKey(pl => pl.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade); // jeśli usuniesz playlistę, jej lajki też znikną

        modelBuilder.Entity<PlaylistLike>()
            .HasOne(pl => pl.User)
            .WithMany(u => u.LikedPlaylists) // user ma kolekcję polubionych playlist
            .HasForeignKey(pl => pl.UserId)
            .OnDelete(DeleteBehavior.Cascade); // jeśli usuniesz usera, jego lajki też znikną

        modelBuilder.Entity<PlaylistValue>()
            .HasKey(pv => new { pv.PlaylistId, pv.MovieId });

        modelBuilder.Entity<PlaylistValue>()
            .HasOne(pv => pv.Playlist)
            .WithMany()
            .HasForeignKey(pv => pv.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlaylistValue>()
            .HasOne(pv => pv.Movie)
            .WithMany()
            .HasForeignKey(pv => pv.MovieId);

        modelBuilder.Entity<PlaylistComment>()
           .HasOne(c => c.User)
           .WithMany(u => u.PlaylistComments)
           .HasForeignKey(c => c.UserId)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PlaylistComment>()
            .HasOne(c => c.Playlist)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlaylistMember>()
            .HasKey(pm => new { pm.PlaylistId, pm.UserId });

        modelBuilder.Entity<PlaylistMember>()
            .HasOne(pm => pm.Playlist)
            .WithMany(p => p.Members)
            .HasForeignKey(pm => pm.PlaylistId);

        modelBuilder.Entity<PlaylistMember>()
            .HasOne(pm => pm.User)
            .WithMany()
            .HasForeignKey(pm => pm.UserId);

        modelBuilder.Entity<PrefferedGenre>()
            .HasKey(pg => new { pg.GenreId, pg.UserId });

        modelBuilder.Entity<PrefferedGenre>()
            .HasOne(pg => pg.Genre)
            .WithMany(g => g.PrefferedGenre)
            .HasForeignKey(pg => pg.GenreId);

        modelBuilder.Entity<PrefferedGenre>()
            .HasOne(pg => pg.User)
            .WithMany(u => u.PrefferedGenres)
            .HasForeignKey(pg => pg.UserId);

        modelBuilder.Entity<Friend>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Friend>()
            .HasOne(f => f.FriendUser)
            .WithMany()
            .HasForeignKey(f => f.FriendId)
            .OnDelete(DeleteBehavior.Restrict);

        //index
        modelBuilder.Entity<PlaylistComment>()
            .HasIndex(c => c.PlaylistId);

        modelBuilder.Entity<UserComment>()
            .HasIndex(c => c.TargetUserId);

        modelBuilder.Entity<Friend>()
            .HasIndex(f => new { f.UserId, f.FriendId })
            .IsUnique();

    }

}