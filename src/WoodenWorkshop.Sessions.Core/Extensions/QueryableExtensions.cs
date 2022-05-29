using WoodenWorkshop.Sessions.Core.Dtos;
using WoodenWorkshop.Sessions.Core.Models;

namespace WoodenWorkshop.Sessions.Core.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<UserSession> ApplyFilter(this IQueryable<UserSession> query, UserSessionsFilter filter)
    {
        var baseQuery = query;
        var (userId, ipAddress, deviceName) = filter.Deconstruct();
        if (userId is not null)
        {
            baseQuery = baseQuery.Where(session => session.UserId == userId);
        }

        if (ipAddress is not null)
        {
            baseQuery = baseQuery.Where(session => session.IpAddress == ipAddress);
        }

        if (deviceName is not null)
        {
            baseQuery = baseQuery.Where(session => session.DeviceName == deviceName);
        }

        return baseQuery;
    }
}