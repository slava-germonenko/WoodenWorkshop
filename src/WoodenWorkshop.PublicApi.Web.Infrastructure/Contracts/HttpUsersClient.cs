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

    public HttpUsersClient(HttpClientFacade httpClient, IOptionsSnapshot<RoutingOptions> routingOptions)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(routingOptions.Value.UsersServiceUrl);
    }

    public async Task<User> GetUserAsync(int userId)
    {
        return await _httpClient.GetAsync<User>(new Uri($"api/users/{userId}", UriKind.RelativeOrAbsolute));
    }

    public async Task<PagedResult<User>> GetUsersAsync(UsersFilter filter)
    {
        return await _httpClient.GetAsync<PagedResult<User>>(new Uri("api/users", UriKind.RelativeOrAbsolute));
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        return await _httpClient.PutAsync<User>(
            new Uri("api/users", UriKind.RelativeOrAbsolute),
            user
        );
    }
}