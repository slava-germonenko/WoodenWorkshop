using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.Sessions.Core.Dtos;

public record UserSessionsFilter : Paging
{
    public int? UserId { get; set; }
    
    public string? IpAddress { get; set; }

    public string? DeviceName { get; set; }
}