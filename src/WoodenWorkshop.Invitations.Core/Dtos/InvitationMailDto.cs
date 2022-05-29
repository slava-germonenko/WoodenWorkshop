namespace WoodenWorkshop.Invitations.Core.Dtos;

public class InvitationMailDto
{
    public string Subject { get; set; } = string.Empty;
    
    public string EmailAddress { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;
}