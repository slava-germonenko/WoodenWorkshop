using WoodenWorkshop.Invitations.Core.Dtos;

namespace WoodenWorkshop.Invitations.Api.Dtos;

public record ApiUserInvitationsFilter : UserInvitationsFilter
{
    public static ValueTask<ApiUserInvitationsFilter?> BindAsync(HttpContext httpContext)
    {
        var query = httpContext.Request.Query;
        var filter = new ApiUserInvitationsFilter
        {
            EmailAddress = query["emailAddress"],
            Search = query["search"]
        };
        
        if (bool.TryParse(query["active"], out var active))
        {
            filter.Active = active;
        }
        
        if (bool.TryParse(query["expired"], out var expired))
        {
            filter.Expired = expired;
        }
        
        filter.Count = int.TryParse(query["count"], out var count) ? count : DefaultCount;

        filter.Offset = int.TryParse(query["offset"], out var offset) ? offset : DefaultOffset;

        return new(filter);
    }
}