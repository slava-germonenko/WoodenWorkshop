namespace WoodenWorkshop.Invitations.Core.Dtos;

public class AcceptUserInvitationDto
{
    public string Token { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}