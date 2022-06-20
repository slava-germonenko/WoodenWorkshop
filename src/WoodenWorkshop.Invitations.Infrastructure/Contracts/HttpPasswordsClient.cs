using Microsoft.Extensions.Options;
using WoodenWorkshop.Common.Utils.Http;
using WoodenWorkshop.Invitations.Core.Contracts;
using WoodenWorkshop.Invitations.Infrastructure.Options;

namespace WoodenWorkshop.Invitations.Infrastructure.Contracts;

public class HttpPasswordsClient : IPasswordsClient
{
    private readonly HttpClientFacade _httpClient;

    private readonly Uri _baseAddress;

    public HttpPasswordsClient(
        HttpClientFacade httpClient,
        IOptions<RoutingOptions> routingOptions
    )
    {
        _httpClient = httpClient;
        _baseAddress = new Uri($"{routingOptions.Value.PasswordsServiceUrl}/api/passwords");
    }

    public async Task SetUserPasswordAsync(int userId, string password)
    {
        var body = new { userId, password };
        await _httpClient.PutAsync(_baseAddress, body);
    }
}