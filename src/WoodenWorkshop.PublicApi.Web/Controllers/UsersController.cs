using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.PublicApi.Web.Core.Contracts;
using WoodenWorkshop.PublicApi.Web.Core.Extensions;
using WoodenWorkshop.PublicApi.Web.Core.Models.Users;

namespace WoodenWorkshop.PublicApi.Web.Controllers;

[ApiController, Authorize, Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUsersClient _usersClient;

    public UsersController(IUsersClient usersClient)
    {
        _usersClient = usersClient;
    }

    [HttpGet("")]
    public async Task<ActionResult<PagedResult<UserViewModel>>> GetUsersAsync(
        [FromQuery] UsersFilter usersFilter
    )
    {
        var users = await _usersClient.GetUsersListForView(usersFilter);
        return Ok(users);
    }

    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserViewModel>> GetUserAsync(int userId)
    {
        var user = await _usersClient.GetUserAsync(userId);
        return Ok(user as UserViewModel);
    }

    [HttpPatch("")]
    public async Task<ActionResult<UserViewModel>> UpdateUserDetailsAsync(UserViewModel user)
    {
        UserViewModel updatedUser = await _usersClient.UpdateUserPersonalDataAsync(user);
        return Ok(updatedUser);
    }
}