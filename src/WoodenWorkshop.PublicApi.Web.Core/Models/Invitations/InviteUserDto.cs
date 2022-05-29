namespace WoodenWorkshop.PublicApi.Web.Core.Models.Invitations;

public record InviteUserDto
{
    public string EmailAddress { get; set; } = string.Empty;

    public DateTime ExpireDate { get; set; } = DateTime.UtcNow.AddDays(1);
}