using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.PublicApi.Web.Core.Models.Sessions;

public record UserSessionsFilter : Paging
{
    public int? UserId { get; set; }
    
    public string? IpAddress { get; set; }

    public string? DeviceName { get; set; }
}