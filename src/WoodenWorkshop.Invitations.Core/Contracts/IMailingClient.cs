using WoodenWorkshop.Invitations.Core.Dtos;

namespace WoodenWorkshop.Invitations.Core.Contracts;

public interface IMailingClient
{
    public Task QueueInvitationEmailAsync(InvitationMailDto invitation);
}