using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.PublicApi.Web.Core.Models.Invitations;

public record UserInvitationsFilter : Paging
{
    public string? Search { get; set; }
    
    public string? EmailAddress { get; set; }
    
    public bool? Active { get; set; }
    
    public bool? Expired { get; set; }
}