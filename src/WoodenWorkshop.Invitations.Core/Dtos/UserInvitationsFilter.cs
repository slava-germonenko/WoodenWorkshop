using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.Invitations.Core.Dtos;

public record UserInvitationsFilter : Paging
{
    public string? Search { get; set; }
    
    public string? EmailAddress { get; set; }
    
    public bool? Active { get; set; }
    
    public bool? Accepted { get; set; }
    
    public bool? Pending { get; set; }
    
    public bool? Expired { get; set; }
}