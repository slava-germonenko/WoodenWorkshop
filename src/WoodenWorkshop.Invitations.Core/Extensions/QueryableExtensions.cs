using WoodenWorkshop.Invitations.Core.Dtos;
using WoodenWorkshop.Invitations.Core.Models;

namespace WoodenWorkshop.Invitations.Core.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<Invitation> ApplyInvitationsFilter(
        this IQueryable<Invitation> baseQuery,
        UserInvitationsFilter filter
    )
    {
        var query = baseQuery;

        if (!string.IsNullOrEmpty(filter.EmailAddress))
        {
            query = query.Where(invitation => invitation.EmailAddress.Equals(filter.EmailAddress));
        }

        if (filter.Active is not null)
        {
            query = query.Where(invitation => invitation.Active == filter.Active);
        }

        query = filter.Expired switch
        {
            true => query.Where(invitation => invitation.ExpireDate <= DateTime.UtcNow),
            false => query.Where(invitation => invitation.ExpireDate > DateTime.UtcNow),
            _ => query
        };

        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(invitation => invitation.EmailAddress.Contains(filter.Search));
        }

        return query;
    }
}