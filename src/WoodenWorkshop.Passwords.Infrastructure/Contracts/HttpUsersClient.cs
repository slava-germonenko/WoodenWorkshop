using Microsoft.Extensions.Options;

using WoodenWorkshop.Common.Utils.Http;
using WoodenWorkshop.Passwords.Core.Contract;
using WoodenWorkshop.Passwords.Core.Dtos;
using WoodenWorkshop.Passwords.Infrastructure.Options;

namespace WoodenWorkshop.Passwords.Infrastructure.Contracts;

public class HttpUsersClient : IUsersClient
{
    private readonly HttpClientFacade _httpClient;

    public HttpUsersClient(
        HttpClientFacade httpClient,
        IOptions<RoutingOptions> routingOptions
    )
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri($"{routingOptions.Value.UsersServiceUrl}");
    }

    public async Task<UserDto> GetUserAsync(int userId)
    {
        return await _httpClient.GetAsync<UserDto>(new Uri($"api/users/{userId}", UriKind.RelativeOrAbsolute));
    }

    public async Task<UserDto> UpdateUserAsync(UserDto user)
    {
        return await _httpClient.PutAsync<UserDto>(new Uri("api/users", UriKind.RelativeOrAbsolute), user);
    }
}