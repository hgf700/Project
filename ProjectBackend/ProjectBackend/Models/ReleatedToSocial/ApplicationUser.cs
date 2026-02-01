using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Core.Types;

namespace ProjectBackend.Models.ReleatedToSocial;

public class ApplicationUser : IdentityUser
{
    public ICollection<UserComment> UserCommentsWritten { get; set; }
    public ICollection<UserComment> UserCommentsReceived { get; set; }
    public ICollection<PlaylistComment> PlaylistComments { get; set; }
}