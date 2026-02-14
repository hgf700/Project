using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Core.Types;
using ProjectBackend.Models.ReleatedToPlaylist;

namespace ProjectBackend.Models.ReleatedToSocial;

public class ApplicationUser : IdentityUser
{
    public ICollection<UserComment> UserCommentsWritten { get; set; }
    public ICollection<UserComment> UserCommentsReceived { get; set; }
    public ICollection<PlaylistComment> PlaylistComments { get; set; }

    public ICollection<UserFollow> Following { get; set; } // kogo obserwuje
    public ICollection<UserFollow> Followers { get; set; } // kto obserwuje

    public ICollection<PlaylistLike> LikedPlaylists { get; set; } = new List<PlaylistLike>();
    public ICollection<PrefferedGenre> PrefferedGenres { get; set; }

}