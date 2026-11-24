namespace Ewallet.Application.DTOs.Dashboard;

public sealed record RecentUserDto(
    long Id,
    string FullName,
    string Email,
    string Status
);

public sealed record AdminDashboardDto(
    int TotalUsers,
    int ActiveUsers,
    int SuspendedUsers,
    IReadOnlyList<RecentUserDto> RecentUsers
);