using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.Common.EntityFramework.Abstractions;

namespace WoodenWorkshop.Common.EntityFramework.Extensions;

public static class QueryableExtensions
{
    public static IOrderedQueryable<T> ApplyOrder<T>(this IQueryable<T> query, IOrderByDto<T> orderByDto)
    {
        return orderByDto.Desc
            ? query.OrderByDescending(orderByDto.KeySelector)
            : query.OrderBy(orderByDto.KeySelector);
    }

    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, Paging paging)
    {
        var total = await query.CountAsync();
        var items = await query.Skip(paging.Offset).Take(paging.Count).ToListAsync();
        return new PagedResult<T>
        {
            Total = total,
            Data = items,
            Offset = paging.Offset,
            Count = paging.Count,
        };
    }
}