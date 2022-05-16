using WoodenWorkshop.Users.Core.Dtos;

namespace WoodenWorkshop.Users.Api.Dtos;

public record ApiUsersFilter : UsersFilter
{
    public static ValueTask<ApiUsersFilter?> BindAsync(HttpContext httpContext)
    {
        var query = httpContext.Request.Query;
        var active = query["active"];
        var filter = new ApiUsersFilter
        {
            FirstName = query["firstName"],
            LastName = query["lastName"],
            EmailAddress = query["emailAddress"],
            Search = query["search"],
            Active = active == "true" || active == "1",
        };
        if (int.TryParse(query["count"], out var count))
        {
            filter.Count = count;
        }

        if (int.TryParse(query["offset"], out var offset))
        {
            filter.Offset = offset;
        }
        
        return new ValueTask<ApiUsersFilter?>(filter);
    }
}