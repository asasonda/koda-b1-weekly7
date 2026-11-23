using System;
using Ewallet.Infrastructure.Identity;

namespace Ewallet.Infrastructure.Identity;

public sealed class RefreshToken
{
    public long Id { get; set; }
    public long AppUserId { get; set; }
    public AppUser User { get; set; } = null!;
    public Guid FamilyId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; }

    public DateTimeOffset? UsedAtUtc { get; set; }
    public DateTimeOffset? RevokedAtUtc { get; set; }
}