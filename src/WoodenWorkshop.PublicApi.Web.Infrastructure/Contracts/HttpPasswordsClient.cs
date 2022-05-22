using Microsoft.Extensions.Options;

using WoodenWorkshop.Common.Utils.Http;
using WoodenWorkshop.PublicApi.Web.Core.Contracts;
using WoodenWorkshop.PublicApi.Web.Infrastructure.Dtos;
using WoodenWorkshop.PublicApi.Web.Infrastructure.Options;

namespace WoodenWorkshop.PublicApi.Web.Infrastructure.Contracts;

public class HttpPasswordsClient : IPasswordsClient
{
    private readonly HttpClientFacade _httpClient;

    public HttpPasswordsClient(HttpClientFacade httpClient, IOptionsSnapshot<RoutingOptions> routingOptions)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(routingOptions.Value.PasswordsServiceUrl);
    }
    
    public async Task<(string passwordHash, string salt)> HashPasswordAsync(string password, string? salt = null)
    {
        var result = await _httpClient.PostAsync<HashPasswordDto>(
            new Uri("api/passwords/hash"),
            new {password, salt}
        );

        return (result.PasswordHash, result.Salt);
    }
}