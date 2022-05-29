using Microsoft.Extensions.Options;

using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.Common.Utils.Http;
using WoodenWorkshop.PublicApi.Web.Core.Contracts;
using WoodenWorkshop.PublicApi.Web.Core.Models.Users;
using WoodenWorkshop.PublicApi.Web.Infrastructure.Options;

namespace WoodenWorkshop.PublicApi.Web.Infrastructure.Contracts;

public class HttpUsersClient : IUsersClient
{
    private readonly HttpClientFacade _httpClient;

    private readonly Uri _baseUsersUri;

    public HttpUsersClient(HttpClientFacade httpClient, IOptionsSnapshot<RoutingOptions> routingOptions)
    {
        _httpClient = httpClient;
        _baseUsersUri = new Uri(routingOptions.Value.UsersServiceUrl);
    }

    public async Task<User> GetUserAsync(int userId)
    {
        return await _httpClient.GetAsync<User>(new Uri(_baseUsersUri, $"api/users/{userId}"));
    }

    public async Task<PagedResult<User>> GetUsersAsync(UsersFilter filter)
    {
        return await _httpClient.GetAsync<PagedResult<User>>(new Uri(_baseUsersUri, "api/users"));
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        return await _httpClient.PutAsync<User>(
            new Uri(_baseUsersUri, "api/users"),
            user
        );
    }
}