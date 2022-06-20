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

    private readonly Uri _baseUri;

    public HttpUsersClient(
        HttpClientFacade httpClient,
        IOptions<RoutingOptions> routingOptions
    )
    {
        _httpClient = httpClient;
        _baseUri = new Uri($"{routingOptions.Value.UsersServiceUrl}/api/users");
    }

    public async Task<UserDto> CreateNewUserAsync(UserDto userDto)
    {
        return await _httpClient.PostAsync<UserDto>(_baseUri, userDto);
    }

    public async Task<UserDto?> GetUserByEmailAddressAsync(string emailAddress)
    {
        var uri = new UriBuilder(_baseUri)
        {
            Query = $"?emailAddress={HttpUtility.UrlEncode(emailAddress)}",
        };
        var users = await _httpClient.GetAsync<PagedResult<UserDto>>(uri.Uri);
        return users.Data.FirstOrDefault();
    }
}