using WoodenWorkshop.Assets.Core.Dtos;

namespace WoodenWorkshop.Assets.Api.Dtos;

public record ApiFoldersFilter : FoldersFilter
{
    public static ValueTask<ApiFoldersFilter?> BindAsync(HttpContext httpContext)
    {
        var query = httpContext.Request.Query;
        var filter = new ApiFoldersFilter
        {
            Search = query["search"],
        };
        
        if (int.TryParse(query["parentId"], out var parentId))
        {
            filter.ParentId = parentId;
        }
            
        filter.Count = int.TryParse(query["count"], out var count) ? count : DefaultCount;

        filter.Offset = int.TryParse(query["offset"], out var offset) ? offset : DefaultOffset;

        return new ValueTask<ApiFoldersFilter?>(filter);
    }
}