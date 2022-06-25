using WoodenWorkshop.Assets.Core.Dtos;

namespace WoodenWorkshop.Assets.Api.Dtos;

public record ApiAssetsFilter : AssetsFilter
{
    public static ValueTask<ApiAssetsFilter?> BindAsync(HttpContext httpContext)
    {
        var query = httpContext.Request.Query;
        var filter = new ApiAssetsFilter
        {
            Search = query["search"],
        };
        
        if (int.TryParse(query["folderId"], out var folderId))
        {
            filter.FolderId = folderId;
        }
            
        filter.Count = int.TryParse(query["count"], out var count) ? count : DefaultCount;

        filter.Offset = int.TryParse(query["offset"], out var offset) ? offset : DefaultOffset;

        return new ValueTask<ApiAssetsFilter?>(filter);
    }
}