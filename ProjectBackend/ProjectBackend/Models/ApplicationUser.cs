using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Core.Types;

namespace ProjectBackend.Models;

public class ApplicationUser : IdentityUser
{

    public bool IsOAuth { get; set; }
    public string? GoogleId { get; set; }

}
