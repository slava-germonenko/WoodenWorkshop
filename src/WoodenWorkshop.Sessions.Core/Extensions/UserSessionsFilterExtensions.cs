using WoodenWorkshop.Sessions.Core.Dtos;

namespace WoodenWorkshop.Sessions.Core.Extensions;

public static class UserSessionsFilterExtensions
{
    public static (int? userId, string? ipAddress, string? deviceName) Deconstruct(
        this UserSessionsFilter sessionsFilter
    ) => (sessionsFilter.UserId, sessionsFilter.IpAddress, sessionsFilter.DeviceName);
}