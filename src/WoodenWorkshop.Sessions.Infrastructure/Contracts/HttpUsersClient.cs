using Microsoft.Extensions.Options;

using WoodenWorkshop.Common.Utils.Http;
using WoodenWorkshop.Sessions.Core.Contracts;
using WoodenWorkshop.Sessions.Core.Models;
using WoodenWorkshop.Sessions.Infrastructure.Options;

namespace WoodenWorkshop.Sessions.Infrastructure.Contracts;

public class HttpUsersClient : IUsersClient
{
    private readonly HttpClientFacade _httpClientFacade;

    private readonly Uri _baseUsersUri;

    public HttpUsersClient(
        HttpClientFacade httpClientFacade,
        IOptionsSnapshot<RoutingOptions> routingOptions
    )
    {
        _httpClientFacade = httpClientFacade;
        _baseUsersUri = new Uri(routingOptions.Value.UsersServiceUrl);
    }

    public Task<User> GetUserAsync(int userId)
    {
        return _httpClientFacade.GetAsync<User>(new Uri(_baseUsersUri, $"api/users/{userId}"));
    }
}