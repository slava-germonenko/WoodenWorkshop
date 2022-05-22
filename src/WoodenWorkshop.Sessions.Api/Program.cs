using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Sessions.Api;
using WoodenWorkshop.Sessions.Api.Dtos;
using WoodenWorkshop.Sessions.Api.Middleware;
using WoodenWorkshop.Sessions.Core;
using WoodenWorkshop.Sessions.Core.Contracts;
using WoodenWorkshop.Sessions.Core.Dtos;
using WoodenWorkshop.Sessions.Infrastructure.Contracts;

var builder = WebApplication.CreateBuilder(args);

var appConfigurationConnectionString = builder.Configuration.GetValue<string>("AppConfigurationConnectionString");
if (!string.IsNullOrEmpty(appConfigurationConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(appConfigurationConnectionString);
}

builder.Services.AddScoped<SessionsService>();
builder.Services.AddScoped<ITokenGenerator, GuidBasedTokenGenerator>();
builder.Services.AddHostedService<SessionsCleanupWorker>();

var coreConnectionString = builder.Configuration.GetValue<string>("CoreSqlConnectionString");
builder.Services.AddDbContext<SessionsContext>(options =>
{
    options.UseSqlServer(coreConnectionString);
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapGet("api/sessions", async (ApiUserSessionsFilter filter, SessionsService sessionsService) =>
{
    var sessions = await sessionsService.GetUserSessionsAsync(filter);
    return Results.Ok(sessions);
});

app.MapPost("api/sessions", async (StartSessionDto sessionDto, SessionsService sessionsService) =>
{
    var startedSession = await sessionsService.StartSessionAsync(sessionDto);
    return Results.Ok(startedSession);
});

app.MapPut("api/sessions", async (RefreshSessionDto sessionDto, SessionsService sessionsService) =>
{
    var startedSession = await sessionsService.RefreshSessionAsync(sessionDto);
    return Results.Ok(startedSession);
});

app.MapDelete("api/sessions/{sessionId:int}", async (int sessionId, SessionsService sessionsService) =>
{
    await sessionsService.TerminateSessionAsync(sessionId);
    return Results.NoContent();
});

app.MapDelete("api/sessions/{token}", async (string token, SessionsService sessionsService) =>
{
    await sessionsService.TerminateSessionAsync(token);
    return Results.NoContent();
});

app.Run();