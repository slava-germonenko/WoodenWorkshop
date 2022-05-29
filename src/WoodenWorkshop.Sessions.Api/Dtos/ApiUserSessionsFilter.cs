using WoodenWorkshop.Sessions.Core.Dtos;

namespace WoodenWorkshop.Sessions.Api.Dtos;

public record ApiUserSessionsFilter : UserSessionsFilter
{
    public static ValueTask<ApiUserSessionsFilter?> BindAsync(HttpContext httpContext)
    {
        var query = httpContext.Request.Query;
        var filter = new ApiUserSessionsFilter
        {
            DeviceName = query["deviceName"],
            IpAddress = query["ipAddress"],
        };

        if (int.TryParse(query["userId"], out var userId))
        {
            filter.UserId = userId;
        }

        if (int.TryParse(query["count"], out var count))
        {
            filter.Count = count;
        }

        if (int.TryParse(query["offset"], out var offset))
        {
            filter.Offset = offset;
        }

        return new(filter);
    }
}