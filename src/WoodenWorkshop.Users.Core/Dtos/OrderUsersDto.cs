using System.Linq.Expressions;

using WoodenWorkshop.Common.EntityFramework.Abstractions;
using WoodenWorkshop.Users.Core.Models;

namespace WoodenWorkshop.Users.Core.Dtos;

public record OrderUsersDto : IOrderByDto<User>
{
    public bool Desc { get; }

    public Expression<Func<User, object>> KeySelector { get; }

    private OrderUsersDto(bool desc, Expression<Func<User, object>> keySelector)
    {
        Desc = desc;
        KeySelector = keySelector;
    }

    public static OrderUsersDto Default() => Create(string.Empty, string.Empty);
    
    public static OrderUsersDto Create(string keyFieldName, string direction)
    {
        var desc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
        Expression<Func<User, object>> keySelector;
        switch (keyFieldName.ToLower())
        {
            case "firstname":
                keySelector = user => user.FirstName;
                break;
            case "lastname":
                keySelector = user => user.LastName;
                break;
            case "emailaddress":
                keySelector = user => user.EmailAddress;
                break;
            default:
                keySelector = user => user.CreatedDate;
                desc = true;
                break;
        }

        return new OrderUsersDto(desc, keySelector);
    }
}