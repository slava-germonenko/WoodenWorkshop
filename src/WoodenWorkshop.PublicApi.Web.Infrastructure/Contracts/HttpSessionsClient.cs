using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.Common.Utils.Http;
using WoodenWorkshop.PublicApi.Web.Core.Contracts;
using WoodenWorkshop.PublicApi.Web.Core.Models.Sessions;
using WoodenWorkshop.PublicApi.Web.Infrastructure.Extensions;
using WoodenWorkshop.PublicApi.Web.Infrastructure.Options;

namespace WoodenWorkshop.PublicApi.Web.Infrastructure.Contracts;

public class HttpSessionsClient : ISessionsClient
{
    private readonly HttpClientFacade _httpClient;

    public HttpSessionsClient(HttpClientFacade httpClient, IOptionsSnapshot<RoutingOptions> routingOptions)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(routingOptions.Value.SessionsServiceUrl);
    }

    public async Task<PagedResult<UserSession>> GetSessionsAsync(UserSessionsFilter filter)
    {
        var url = QueryHelpers.AddQueryString("api/sessions", filter.ToQueryDictionary());
        return await _httpClient.GetAsync<PagedResult<UserSession>>(new Uri(url, UriKind.RelativeOrAbsolute));
    }

    public async Task<UserSession> StartSessionAsync(StartSessionDto sessionDto)
    {
        return await _httpClient.PostAsync<UserSession>(
            new Uri("api/sessions", UriKind.RelativeOrAbsolute),
            sessionDto
        );
    }

    public async Task<UserSession> RefreshSessionAsync(RefreshSessionDto sessionDto)
    {
        return await _httpClient.PutAsync<UserSession>(
            new Uri("api/sessions", UriKind.RelativeOrAbsolute),
            sessionDto
        );
    }

    public async Task TerminateSessionAsync(string refreshToken)
    {
        await _httpClient.DeleteAsync(
            new Uri($"api/sessions/{refreshToken}", UriKind.RelativeOrAbsolute)
        );
    }
}