using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.Users.Core.Dtos;

public record UsersFilter : Paging
{
    public string? EmailAddress { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }

    public string? Search { get; set; }
    
    public bool? Active { get; set; }
}
