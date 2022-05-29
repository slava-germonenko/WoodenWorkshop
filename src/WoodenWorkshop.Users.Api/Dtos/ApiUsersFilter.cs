using WoodenWorkshop.Users.Core.Dtos;

namespace WoodenWorkshop.Users.Api.Dtos;

public record ApiUsersFilter : UsersFilter
{
    public static ValueTask<ApiUsersFilter?> BindAsync(HttpContext httpContext)
    {
        var query = httpContext.Request.Query;
        var filter = new ApiUsersFilter
        {
            FirstName = query["firstName"],
            LastName = query["lastName"],
            EmailAddress = query["emailAddress"],
            Search = query["search"],
        };

        if (bool.TryParse(query["active"], out var active))
        {
            filter.Active = active;
        }
        
        filter.Count = int.TryParse(query["count"], out var count) ? count : DefaultCount;

        filter.Offset = int.TryParse(query["offset"], out var offset) ? offset : DefaultOffset;
        
        return new ValueTask<ApiUsersFilter?>(filter);
    }
}