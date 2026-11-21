using Microsoft.AspNetCore.Identity;

namespace Ewallet.Infrastructure.Identity;

public sealed class AppUser : IdentityUser<long>
{
    public string FullName {get; set;} = string.Empty;
    public string Status {get; set;} = "ACTIVE";
    public DateTimeOffset CreatedAtUtc {get; set;} = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc {get; set;}
}