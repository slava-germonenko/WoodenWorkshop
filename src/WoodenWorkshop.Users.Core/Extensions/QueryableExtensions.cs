using WoodenWorkshop.Users.Core.Dtos;
using WoodenWorkshop.Users.Core.Models;

namespace WoodenWorkshop.Users.Core.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<User> ApplyUsersFilter(this IQueryable<User> query, UsersFilter filter)
    {
        var baseQuery = query;
        if (!string.IsNullOrEmpty(filter.FirstName))
        {
            baseQuery = baseQuery.Where(user => user.FirstName.Equals(filter.FirstName));
        }

        if (!string.IsNullOrEmpty(filter.LastName))
        {
            baseQuery = baseQuery.Where(user => user.LastName.Equals(filter.LastName));
        }

        if (!string.IsNullOrEmpty(filter.EmailAddress))
        {
            baseQuery = baseQuery.Where(user => user.EmailAddress.Equals(filter.EmailAddress));
        }

        if (filter.Active is not null)
        {
            baseQuery = baseQuery.Where(user => user.Active == filter.Active);
        }

        var search = filter.Search;
        if (!string.IsNullOrEmpty(search))
        {
            baseQuery = baseQuery.Where(
                user => user.FirstName.Contains(search) 
                        || user.LastName.Contains(search)
                        || user.EmailAddress.Contains(search)
            );
        }
            
        return baseQuery;
    }
}