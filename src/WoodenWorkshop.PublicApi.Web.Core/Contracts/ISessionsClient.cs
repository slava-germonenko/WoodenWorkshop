using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.PublicApi.Web.Core.Models.Sessions;

namespace WoodenWorkshop.PublicApi.Web.Core.Contracts;

public interface ISessionsClient
{
    public Task<PagedResult<UserSession>> GetSessionsAsync(UserSessionsFilter filter);

    public Task<UserSession> StartSessionAsync(StartSessionDto sessionDto);

    public Task<UserSession> RefreshSessionAsync(RefreshSessionDto sessionDto);

    public Task TerminateSessionAsync(string refreshToken);
}