using WoodenWorkshop.PublicApi.Web.Core.Models.Auth;
using WoodenWorkshop.PublicApi.Web.Core.Models.Sessions;

namespace WoodenWorkshop.PublicApi.Web.Core.Extensions;

public static class UserSessionExtensions
{
    public static RefreshToken ToRefreshToken(this UserSession session) => new()
    {
        Token = session.Token,
        ExpireDate = session.ExpireDate,
    };
}