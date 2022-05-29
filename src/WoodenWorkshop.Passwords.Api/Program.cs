using WoodenWorkshop.Common.Utils.Http;
using WoodenWorkshop.Passwords.Api.Dtos;
using WoodenWorkshop.Passwords.Core;
using WoodenWorkshop.Passwords.Core.Contract;
using WoodenWorkshop.Passwords.Infrastructure.Contracts;
using WoodenWorkshop.Passwords.Infrastructure.Options;
using WoodenWorkshop.Users.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

var appConfigurationConnectionString = builder.Configuration.GetValue<string>("AppConfigurationConnectionString");
if (!string.IsNullOrEmpty(appConfigurationConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(appConfigurationConnectionString);
}

builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection("Security"));
builder.Services.Configure<RoutingOptions>(builder.Configuration.GetSection("Routing"));
builder.Services.AddScoped<PasswordsService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUsersClient, HttpUsersClient>();
builder.Services.AddScoped<HttpClientFacade>();
builder.Services.AddHttpClient();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapPut("/api/passwords", async (SetPasswordDto passwordDto, PasswordsService passwordsService) =>
{
    await passwordsService.SetUserPasswordAsync(passwordDto.UserId, passwordDto.Password);
    return Results.NoContent();
});

app.MapPost("/api/passwords/hash", (HashPasswordDto hashPasswordDto,PasswordsService passwordsService) =>
{
    var (passwordHash, salt) = passwordsService.GeneratePasswordHash(
        hashPasswordDto.Password,
        hashPasswordDto.Salt
    );

    return Results.Ok(new {passwordHash, salt});
});

app.Run();