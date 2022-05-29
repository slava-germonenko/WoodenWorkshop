namespace WoodenWorkshop.Invitations.Core.Dtos;

public class AcceptUserInvitationData
{
    public string Token { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}