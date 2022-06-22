using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Assets.Core;
using WoodenWorkshop.Assets.Core.Contracts;
using WoodenWorkshop.Assets.Infrastructure.Contracts;
using WoodenWorkshop.Assets.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);
var appConfigurationConnectionString = builder.Configuration.GetValue<string>("AppConfigurationConnectionString");
if (!string.IsNullOrEmpty(appConfigurationConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(appConfigurationConnectionString);
}

builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection("Storage"));
builder.Services.AddScoped<AssetsService>();
builder.Services.AddScoped<FoldersCleanupService>();
builder.Services.AddScoped<FoldersService>();
builder.Services.AddScoped<IBlobStorage, AzureBlobStorage>();
builder.Services.AddScoped<IBulkFoldersRepository, SqlServerBulkFoldersRepository>();

builder.Services.AddDbContext<AssetsContext>(options =>
{
    var connectionString = builder.Configuration.GetValue<string>("CoreSqlConnectionString");
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

app.Run();