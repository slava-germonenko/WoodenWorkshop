using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.PublicApi.Web.Core.Models.Invitations;

namespace WoodenWorkshop.PublicApi.Web.Core.Contracts;

public interface IUserInvitationsClient
{
    Task<PagedResult<Invitation>> GetInvitationsAsync(UserInvitationsFilter filter);

    Task<Invitation> GetInvitationAsync(string uniqueToken);

    Task<Invitation> InviteUserAsync(InviteUserDto invitation);

    Task<Invitation> UpdateInvitationAsync(Invitation invitation);

    Task AcceptInvitationAsync(AcceptUserInvitationDto acceptInvitationDto);

    Task DeclineInvitationAsync(string token);
}