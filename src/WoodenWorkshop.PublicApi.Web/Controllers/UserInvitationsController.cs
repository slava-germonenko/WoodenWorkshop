using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WoodenWorkshop.PublicApi.Web.Core.Contracts;
using WoodenWorkshop.PublicApi.Web.Core.Models.Invitations;

namespace WoodenWorkshop.PublicApi.Web.Controllers;

[ApiController, Route("api/user-invitations")]
public class UserInvitationsController : ControllerBase
{
    private readonly IUserInvitationsClient _userInvitationsClient;

    public UserInvitationsController(IUserInvitationsClient userInvitationsClient)
    {
        _userInvitationsClient = userInvitationsClient;
    }

    [HttpPost(""), Authorize]
    public async Task<ActionResult<Invitation>> InviteUserAsync(InviteUserDto invitationDto)
    {
        var invitation = await _userInvitationsClient.InviteUserAsync(invitationDto);
        return Ok(invitation);
    }

    [HttpPost("{token}/accept")]
    public async Task<NoContentResult> AcceptInvitationAsync(string token, AcceptUserInvitationDto acceptData)
    {
        acceptData.Token = token;
        await _userInvitationsClient.AcceptInvitationAsync(acceptData);
        return NoContent();
    }

    [HttpPost("{token}/decline")]
    public async Task<NoContentResult> DeclineInvitationAsync(string token)
    {
        await _userInvitationsClient.DeclineInvitationAsync(token);
        return NoContent();
    }
}