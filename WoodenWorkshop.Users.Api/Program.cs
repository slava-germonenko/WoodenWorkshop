using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Users.Core;
using WoodenWorkshop.Users.Core.Models;

var builder = WebApplication.CreateBuilder(args);
var appConfigurationConnectionString = builder.Configuration.GetValue<string>("AppConfigurationConnectionString");
if (!string.IsNullOrEmpty(appConfigurationConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(appConfigurationConnectionString);
}

builder.Services.AddScoped<UsersService>();

var coreConnectionString = builder.Configuration.GetValue<string>("CoreSqlConnectionString");
builder.Services.AddDbContext<UsersContext>(options => options.UseSqlServer(coreConnectionString));

var app = builder.Build();

app.MapGet("/api/users/{userId:int}", async (int userId, UsersService usersService) =>
{
    var user = await usersService.GetUserAsync(userId);
    return Results.Ok(user);
});

app.MapPut("/api/users", async ([FromBody] User user, UsersService usersService) =>
{
    var savedUser = await usersService.SaveUserAsync(user);
    return Results.Ok(savedUser);
});

app.MapDelete("/api/users/{userId:int}", async (int userId, UsersService usersService) =>
{
    await usersService.RemoveUserAsync(userId);
    return Results.NoContent();
});

app.Run();