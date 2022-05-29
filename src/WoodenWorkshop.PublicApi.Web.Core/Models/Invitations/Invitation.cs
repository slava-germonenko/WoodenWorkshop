using System.ComponentModel.DataAnnotations;

using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.PublicApi.Web.Core.Models.Invitations.Enums;

namespace WoodenWorkshop.PublicApi.Web.Core.Models.Invitations;

public class Invitation : BaseModel
{
    public InvitationTypes Type { get; set; } = InvitationTypes.NewUser;

    [Required(ErrorMessage = "Адрес электронной почты – обязательное поле.")] 
    [StringLength(250, ErrorMessage = "Максимальная длина адреса электронной почты – 250 символов.")] 
    public string EmailAddress { get; set; } = string.Empty;

    public DateTime ExpireDate { get; set; }

    public bool Active { get; set; } = true;
    
    public bool? Accepted { get; set; }
}