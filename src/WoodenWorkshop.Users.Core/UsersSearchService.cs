using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.Common.EntityFramework.Extensions;
using WoodenWorkshop.Users.Core.Dtos;
using WoodenWorkshop.Users.Core.Extensions;
using WoodenWorkshop.Users.Core.Models;

namespace WoodenWorkshop.Users.Core;

public class UsersSearchService
{
    private readonly UsersContext _usersContext;

    public UsersSearchService(UsersContext usersContext)
    {
        _usersContext = usersContext;
    }

    public async Task<PagedResult<User>> SearchUsersAsync(
        UsersFilter filter,
        OrderUsersDto orderUsersDto
    )
    {
        return await _usersContext.Users.AsNoTracking()
            .ApplyOrder(orderUsersDto)
            .ApplyUsersFilter(filter)
            .ToPagedResult(filter);
    }
}