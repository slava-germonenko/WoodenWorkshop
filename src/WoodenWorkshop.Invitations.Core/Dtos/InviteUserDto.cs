using WoodenWorkshop.Invitations.Core.Models;

namespace WoodenWorkshop.Invitations.Core.Dtos;

public record InviteUserDto
{
    public string EmailAddress { get; set; } = string.Empty;

    public DateTime ExpireDate { get; set; } = DateTime.UtcNow.AddDays(1);

    public Invitation ToInvitation() => new()
    {
        EmailAddress = EmailAddress,
        ExpireDate = ExpireDate,
    };
}