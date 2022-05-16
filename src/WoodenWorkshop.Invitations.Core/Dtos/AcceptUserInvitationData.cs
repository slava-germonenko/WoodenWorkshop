namespace WoodenWorkshop.Invitations.Core.Dtos;

public class AcceptUserInvitationData
{
    public int InvitationId { get; set; }
    
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}