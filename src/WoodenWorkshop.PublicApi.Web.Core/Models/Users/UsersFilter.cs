using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.PublicApi.Web.Core.Models.Users;

public record UsersFilter : Paging
{
    public string? EmailAddress { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }

    public string? Search { get; set; }
    
    public bool? Active { get; set; }
}