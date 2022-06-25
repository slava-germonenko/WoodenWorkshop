using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using WoodenWorkshop.Assets.Api;
using WoodenWorkshop.Assets.Api.Dtos;
using WoodenWorkshop.Assets.Api.Middleware;
using WoodenWorkshop.Assets.Core;
using WoodenWorkshop.Assets.Core.Contracts;
using WoodenWorkshop.Assets.Core.Dtos;
using WoodenWorkshop.Assets.Core.Models;
using WoodenWorkshop.Assets.Infrastructure.Contracts;
using WoodenWorkshop.Assets.Infrastructure.Options;
using WoodenWorkshop.Common.Core.Models;

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

builder.Services.AddHostedService<FoldersCleanupWorker>();

var blobStorageConnectionString = builder.Configuration.GetValue<string>("BlobStorageConnectionString");
builder.Services.AddAzureClients(services =>
{
    services.AddBlobServiceClient(blobStorageConnectionString);
});
builder.Services.AddDbContext<AssetsContext>(options =>
{
    var connectionString = builder.Configuration.GetValue<string>("CoreSqlConnectionString");
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapGet("api/assets", async Task<IResult>(
    [FromQuery] string? direction,
    [FromQuery] string? order,
    ApiAssetsFilter filter,
    AssetsService service
) =>
{
    var orderBy = AssetsOrder.Create(order ?? string.Empty, direction ?? string.Empty);
    var assets = await service.GetAssetsAsync(filter, orderBy);
    return Results.Ok(app);
});

app.MapPut("api/assets/{assetId:int}/details", async Task<IResult>(
    int assetId,
    AssetDetailsDto assetDetailsDto,
    AssetsService service
) =>
{
    assetDetailsDto.AssetId = assetId;
    var asset = await service.UpdateAssetDetailsAsync(assetDetailsDto);
    return Results.Ok(asset);
});

app.MapPut("api/assets/{assetId:int}/blob", async Task<IResult>(
    int assetId,
    IFormFile file,
    AssetsService service
) =>
{
    if (file.Length == 0)
    {
        return Results.BadRequest(new BaseError{ ErrorCode = 0, Message = "Необходимо прикрепить файл."});
    }

    var fileContent = file.OpenReadStream();
    var asset = await service.ReplaceAssetContentAsync(assetId, fileContent);
    return Results.Ok(asset);
});

app.MapDelete("api/assets/{assetId:int}", async (int assetId, AssetsService service) =>
{
    await service.RemoveAssetAsync(assetId);
    return Results.NoContent();
});

app.MapPost("api/folders/{folderId:int}/assets", async (
    int folderId,
    IFormFile file,
    AssetsService assetsService
) =>
{
    if (file.Length == 0)
    {
        return Results.BadRequest(new BaseError{ ErrorCode = 0, Message = "Необходимо прикрепить файл."});
    }

    var fileContent = file.OpenReadStream();
    var addAssetDto = new AddAssetDto(file.Name, fileContent, folderId);
    var asset = await assetsService.AddAssetAsync(addAssetDto);
    return Results.Ok(asset);
});

app.MapPost("api/assets", async (IFormFile file, AssetsService assetsService) =>
{
    if (file.Length == 0)
    {
        return Results.BadRequest(new BaseError{ ErrorCode = 0, Message = "Необходимо прикрепить файл."});
    }

    var fileContent = file.OpenReadStream();
    var addAssetDto = new AddAssetDto(file.Name, fileContent);
    var asset = await assetsService.AddAssetAsync(addAssetDto);
    return Results.Ok(asset);
});

app.MapGet("api/folders", async (
    [FromQuery] string? direction,
    [FromQuery] string? order,
    ApiFoldersFilter filter,
    FoldersService service) =>
{
    var orderBy = FoldersOrder.Create(order ?? string.Empty, direction ?? string.Empty);
    var assets = await service.GetFoldersAsync(filter, orderBy);
    return Results.Ok(app);
});

app.MapPost("api/folders", async (Folder folder, FoldersService foldersService) =>
{
    folder.Id = default;
    var savedFolder = await foldersService.SaveFolderAsync(folder);
    return Results.Ok(savedFolder);
});

app.MapPut("api/folders", async (Folder folder, FoldersService foldersService) =>
{
    var savedFolder = await foldersService.SaveFolderAsync(folder);
    return Results.Ok(savedFolder);
});

app.MapDelete("api/folders/{folderId:int}", async Task<IResult>(
    int folderId,
    FoldersService foldersService
) =>
{
    await foldersService.MarkFolderWithContentAsDeleted(folderId);
    return Results.NoContent();
});

app.Run();