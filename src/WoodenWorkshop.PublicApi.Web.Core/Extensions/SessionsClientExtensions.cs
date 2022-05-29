using WoodenWorkshop.PublicApi.Web.Core.Contracts;
using WoodenWorkshop.PublicApi.Web.Core.Models.Sessions;

namespace WoodenWorkshop.PublicApi.Web.Core.Extensions;

public static class SessionsClientExtensions
{
    public static async Task<UserSession?> GetDeviceSessionAsync(
        this ISessionsClient client,
        int userId,
        string ipAddress,
        string deviceName
    )
    {
        var filter = new UserSessionsFilter
        {
            UserId = userId,
            DeviceName = deviceName,
            IpAddress = ipAddress,
            Count = 1,
        };

        var response = await client.GetSessionsAsync(filter);
        return response.Data.FirstOrDefault();
    }
}