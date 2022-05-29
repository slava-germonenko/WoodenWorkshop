namespace WoodenWorkshop.Invitations.Core.Contracts;

public interface IUserInvitationEmailCompiler
{
    public Task<string> CompileUserInvitationAsync(string invitationToken);
}