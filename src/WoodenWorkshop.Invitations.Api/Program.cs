using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using RabbitMQ.Client;

using WoodenWorkshop.Common.Utils.Http;
using WoodenWorkshop.Invitations.Api.Dtos;
using WoodenWorkshop.Invitations.Core;
using WoodenWorkshop.Invitations.Core.Contracts;
using WoodenWorkshop.Invitations.Core.Dtos;
using WoodenWorkshop.Invitations.Core.Models;
using WoodenWorkshop.Invitations.Infrastructure.Contracts;
using WoodenWorkshop.Invitations.Infrastructure.Options;
using WoodenWorkshop.Users.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
var appConfigurationConnectionString = builder.Configuration.GetValue<string>("AppConfigurationConnectionString");
if (!string.IsNullOrEmpty(appConfigurationConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(appConfigurationConnectionString);
}

builder.Services.Configure<MailingOptions>(builder.Configuration.GetSection("Mailing"));
builder.Services.Configure<RoutingOptions>(builder.Configuration.GetSection("Routing"));
builder.Services.AddScoped<IUserInvitationEmailCompiler, HandlebarsUserInvitationEmailCompiler>();
builder.Services.AddScoped<IPasswordsClient, HttpPasswordsClient>();
builder.Services.AddScoped<IUsersClient, HttpUsersClient>();
builder.Services.AddScoped<IMailingClient, MessageQueueMailingClient>();
builder.Services.AddScoped<ITokenGenerator, GuidBasedTokenGenerator>();
builder.Services.AddScoped<UserInvitationsService>();
builder.Services.AddScoped<HttpClientFacade>();
builder.Services.AddHttpClient();

var blobStorageConnectionString = builder.Configuration.GetValue<string>("BlobStorageConnectionString");
builder.Services.AddAzureClients(services =>
{
    services.AddBlobServiceClient(blobStorageConnectionString);
});

var coreConnectionString = builder.Configuration.GetValue<string>("CoreSqlConnectionString");
builder.Services.AddDbContext<InvitationsContext>(options =>
{
    options.UseSqlServer(coreConnectionString);
});

var messageQueueConfiguration = builder.Configuration.GetSection("MessageQueue");
builder.Services.AddSingleton<IConnectionFactory>(
    _ => new ConnectionFactory
    {
        HostName = messageQueueConfiguration.GetValue<string>("Host"),
        UserName = messageQueueConfiguration.GetValue<string>("Username"),
        Password = messageQueueConfiguration.GetValue<string>("Password"),
    }
);

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();


app.MapGet("api/user-invitations", async (ApiUserInvitationsFilter filter, UserInvitationsService invitationsService) =>
{
    var invitationsPage = await invitationsService.GetUserInvitationsAsync(filter);
    return Results.Ok(invitationsPage);
});

app.MapGet("api/user-invitations/{uniqueToken}", async (string uniqueToken, UserInvitationsService invitationsService) =>
{
    var invitation = await invitationsService.GetUserInvitationAsync(uniqueToken);
    return Results.Ok(invitation);
});

app.MapPost("api/user-invitations", async (InviteUserDto inviteUserDto, UserInvitationsService invitationsService) =>
{
    var invitation = await invitationsService.InviteUserAsync(inviteUserDto);
    return Results.Ok(invitation);
});

app.MapPut("api/user-invitations", async (Invitation invitation, UserInvitationsService invitationsService) =>
{
    var updatedInvitation = await invitationsService.UpdateUserInvitation(invitation);
    return Results.Ok(updatedInvitation);
});

app.MapPost("api/user-invitations/{token}/decline", async (string token, UserInvitationsService invitationsService) =>
{
    await invitationsService.DeclineUserInvitationAsync(token);
    return Results.NoContent();
});

app.MapPost(
    "api/user-invitations/{token}/accept", 
    async (string token, AcceptUserInvitationData acceptUserInvitationData, UserInvitationsService invitationsService
) =>
    {
        acceptUserInvitationData.Token = token;
        await invitationsService.AcceptUserInvitationAsync(acceptUserInvitationData);
    });

app.Run();