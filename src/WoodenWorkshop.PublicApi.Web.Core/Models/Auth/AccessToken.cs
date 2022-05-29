namespace WoodenWorkshop.PublicApi.Web.Core.Models.Auth;

public record AccessToken
{
    public int UserId { get; set; }
    
    public DateTime ExpireDate { get; set; }

    public string Token { get; set; } = string.Empty;
}