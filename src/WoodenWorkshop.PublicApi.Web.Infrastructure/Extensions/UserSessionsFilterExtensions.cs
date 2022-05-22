using Microsoft.Extensions.Primitives;

using WoodenWorkshop.PublicApi.Web.Core.Models.Sessions;

namespace WoodenWorkshop.PublicApi.Web.Infrastructure.Extensions;

public static class UserSessionsFilterExtensions
{
    public static Dictionary<string, StringValues> ToQueryDictionary(this UserSessionsFilter filter)
    {
        var queryDictionary = new Dictionary<string, StringValues>
        {
            {"count", filter.Count.ToString()},
            {"offset", filter.Offset.ToString()}
        };
        if (filter.UserId is not null)
        {
            queryDictionary.Add("userId", filter.UserId.ToString());
        }

        if (filter.IpAddress is not null)
        {
            queryDictionary.Add("ipAddress", filter.IpAddress);
        }

        if (filter.DeviceName is not null)
        {
            queryDictionary.Add("deviceName", filter.DeviceName);
        }
        return queryDictionary;
    }
}