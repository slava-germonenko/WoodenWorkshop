using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using WoodenWorkshop.Common.Utils.Http;
using WoodenWorkshop.PublicApi.Web.Core.Contracts;
using WoodenWorkshop.PublicApi.Web.Core.Options;
using WoodenWorkshop.PublicApi.Web.Core.Services;
using WoodenWorkshop.PublicApi.Web.Infrastructure.Contracts;
using WoodenWorkshop.PublicApi.Web.Infrastructure.Options;
using WoodenWorkshop.PublicApi.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);
var appConfigConnectionString = builder.Configuration.GetValue<string>("AppConfigurationConnectionString");
if (!string.IsNullOrEmpty(appConfigConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(appConfigConnectionString);
}

var jwtSecret = builder.Configuration.GetValue<string>("Security:JwtSecret");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        };
    });

builder.Services.AddControllers();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.Configure<RoutingOptions>(builder.Configuration.GetSection("Routing"));
builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection("Security"));
builder.Services.AddScoped<WebApiAuthService>();
builder.Services.AddScoped<IPasswordsClient, HttpPasswordsClient>();
builder.Services.AddScoped<ISessionsClient, HttpSessionsClient>();
builder.Services.AddScoped<IUsersClient, HttpUsersClient>();
builder.Services.AddScoped<HttpClientFacade>();
builder.Services.AddHttpClient();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();