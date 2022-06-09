using System.ComponentModel.DataAnnotations;

using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.Invitations.Core.Models.Enums;

namespace WoodenWorkshop.Invitations.Core.Models;

public class Invitation : BaseModel
{
    public InvitationTypes Type { get; set; } = InvitationTypes.NewUser;

    [Required(ErrorMessage = "Адрес электронной почты – обязательное поле.")] 
    [StringLength(250, ErrorMessage = "Максимальная длина адреса электронной почты – 250 символов.")] 
    public string EmailAddress { get; set; } = string.Empty;

    public string UniqueToken { get; set; } = string.Empty;

    public DateTime ExpireDate { get; set; }

    public bool Active { get; set; } = true;
    
    public bool? Accepted { get; set; }

    public void MarkAsAccepted(bool accept)
    {
        Active = false;
        Accepted = accept;
    }

    public void CopyDetails(Invitation source)
    {
        ExpireDate = source.ExpireDate;
        Active = source.Active;
    }
}