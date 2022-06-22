using WoodenWorkshop.Assets.Core.Dtos;
using WoodenWorkshop.Assets.Core.Models;

namespace WoodenWorkshop.Assets.Core.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<Asset> ApplyFilters(
        this IQueryable<Asset> baseQuery,
        AssetsFilter filter
    )
    {
        var query = baseQuery.Where(
            asset => asset.FolderId == filter.FolderId && asset.DeletedDate == null
        );

        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(asset => asset.Name.Contains(filter.Search));
        }

        return query;
    }
    
    public static IQueryable<Folder> ApplyFilters(
        this IQueryable<Folder> baseQuery,
        FoldersFilter filter
    )
    {
        var query = baseQuery.Where(
            folder => folder.ParentId == filter.ParentId && folder.DeletedDate == null
        );

        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(folder => folder.Name.Contains(filter.Search));
        }

        return query;
    }
}