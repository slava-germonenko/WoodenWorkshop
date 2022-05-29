namespace WoodenWorkshop.PublicApi.Web.Core.Models.Invitations;

public record AcceptUserInvitationDto
{
    public string Token { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}