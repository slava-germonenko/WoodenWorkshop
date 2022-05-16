using System.Web;

using Microsoft.Extensions.Options;

using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.Common.Utils.Http;
using WoodenWorkshop.Invitations.Core.Contracts;
using WoodenWorkshop.Invitations.Core.Dtos;
using WoodenWorkshop.Invitations.Infrastructure.Options;

namespace WoodenWorkshop.Invitations.Infrastructure.Contracts;

public class HttpUsersClient : IUsersClient
{
    private readonly HttpClientFacade _httpClient;

    public HttpUsersClient(
        HttpClientFacade httpClient,
        IOptions<RoutingOptions> routingOptions
    )
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri($"{routingOptions.Value.UsersServiceUrl}/api/users");
    }

    public async Task<UserDto> CreateNewUserAsync(UserDto userDto)
    {
        return await _httpClient.PostAsync<UserDto>(null, userDto);
    }

    public async Task<UserDto?> GetUserByEmailAddressAsync(string emailAddress)
    {
        var query = $"?emailAddress={HttpUtility.UrlEncode(emailAddress)}";
        var users = await _httpClient.GetAsync<PagedResult<UserDto>>(new Uri(query, UriKind.RelativeOrAbsolute));
        return users.Data.FirstOrDefault();
    }
}