using Microsoft.Extensions.Options;

using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.Common.Utils.Http;
using WoodenWorkshop.Common.Utils.Http.Query;
using WoodenWorkshop.PublicApi.Web.Core.Contracts;
using WoodenWorkshop.PublicApi.Web.Core.Models.Sessions;
using WoodenWorkshop.PublicApi.Web.Infrastructure.Options;

namespace WoodenWorkshop.PublicApi.Web.Infrastructure.Contracts;

public class HttpSessionsClient : ISessionsClient
{
    private readonly HttpClientFacade _httpClient;

    private readonly Uri _baseSessionsUri;
    
    private readonly QueryBuilder _queryBuilder = new();

    public HttpSessionsClient(HttpClientFacade httpClient, IOptionsSnapshot<RoutingOptions> routingOptions)
    {
        _httpClient = httpClient;
        _baseSessionsUri = new Uri(routingOptions.Value.SessionsServiceUrl);
    }

    public async Task<PagedResult<UserSession>> GetSessionsAsync(UserSessionsFilter filter)
    {
        var uriBuilder = new UriBuilder(new Uri(_baseSessionsUri, "api/sessions"))
        {
            Query = _queryBuilder.BuildQuery(filter)
        };
        Console.WriteLine(uriBuilder.Uri.ToString());
        return await _httpClient.GetAsync<PagedResult<UserSession>>(uriBuilder.Uri);
    }

    public Task<UserSession> StartSessionAsync(StartSessionDto sessionDto)
    {
        return _httpClient.PostAsync<UserSession>(
            new Uri(_baseSessionsUri, "api/sessions"),
            sessionDto
        );
    }

    public async Task<UserSession> RefreshSessionAsync(RefreshSessionDto sessionDto)
    {
        return await _httpClient.PutAsync<UserSession>(
            new Uri(_baseSessionsUri, "api/sessions"),
            sessionDto
        );
    }

    public async Task TerminateSessionAsync(string refreshToken)
    {
        await _httpClient.DeleteAsync(
            new Uri(_baseSessionsUri, $"api/sessions/{refreshToken}")
        );
    }
}