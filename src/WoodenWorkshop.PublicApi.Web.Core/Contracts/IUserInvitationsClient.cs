using WoodenWorkshop.PublicApi.Web.Core.Models.Invitations;

namespace WoodenWorkshop.PublicApi.Web.Core.Contracts;

public interface IUserInvitationsClient
{
    Task<Invitation> InviteUserAsync(InviteUserDto invitation);

    Task AcceptInvitationAsync(AcceptUserInvitationDto acceptInvitationDto);

    Task DeclineInvitationAsync(string token);
}