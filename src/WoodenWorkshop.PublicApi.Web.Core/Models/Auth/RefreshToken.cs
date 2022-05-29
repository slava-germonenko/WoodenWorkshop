namespace WoodenWorkshop.PublicApi.Web.Core.Models.Auth;

public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    
    public DateTime? ExpireDate { get; set; }
}