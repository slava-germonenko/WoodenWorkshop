using Microsoft.Extensions.Options;

using WoodenWorkshop.Common.Utils.Http;
using WoodenWorkshop.PublicApi.Web.Core.Contracts;
using WoodenWorkshop.PublicApi.Web.Core.Models.Invitations;
using WoodenWorkshop.PublicApi.Web.Infrastructure.Options;

namespace WoodenWorkshop.PublicApi.Web.Infrastructure.Contracts;

public class HttpUserInvitationsClient : IUserInvitationsClient
{
    private readonly HttpClientFacade _httpClient;

    private readonly Uri _baseSessionsUri;

    public HttpUserInvitationsClient(HttpClientFacade httpClient, IOptionsSnapshot<RoutingOptions> routingOptions)
    {
        _httpClient = httpClient;
        _baseSessionsUri = new Uri(routingOptions.Value.InvitationsServiceUrl);
    }
    
    public Task<Invitation> InviteUserAsync(InviteUserDto invitation)
    {
        return _httpClient.PostAsync<Invitation>(
            new Uri(_baseSessionsUri, "api/user-invitations"),
            invitation
        );
    }

    public Task AcceptInvitationAsync(AcceptUserInvitationDto acceptInvitationDto)
    {
        return _httpClient.PostAsync(
            new Uri(_baseSessionsUri, $"api/user-invitations/{acceptInvitationDto.Token}/accept"),
            acceptInvitationDto
        );
    }

    public Task DeclineInvitationAsync(string token)
    {
        return _httpClient.PostAsync(new Uri(_baseSessionsUri, $"api/user-invitations/{token}/decline"));
    }
}