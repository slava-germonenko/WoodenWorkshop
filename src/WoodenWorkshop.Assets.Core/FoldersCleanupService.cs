using Microsoft.EntityFrameworkCore;

namespace WoodenWorkshop.Assets.Core;

public class FoldersCleanupService
{
    private const int FoldersCleanupBatch = 100;
    
    private readonly AssetsContext _assetsContext;

    private readonly AssetsService _assetsService;

    public FoldersCleanupService(
        AssetsContext assetsContext,
        AssetsService assetsService)
    {
        _assetsContext = assetsContext;
        _assetsService = assetsService;
    }

    public async Task CleanupFoldersAsync()
    {
        var folders = await _assetsContext.Folders.Where(f => f.DeletedDate != null)
            .AsNoTracking()
            .OrderBy(f => f.DeletedDate)
            .Take(FoldersCleanupBatch)
            .ToListAsync();


        foreach (var folder in folders)
        {
            await RemoveFolderWithContentAsync(folder.Id);
        }
    }

    private async Task RemoveFolderWithContentAsync(int folderId)
    {
        var childFolders = await _assetsContext.Folders.AsNoTracking()
            .Where(f => f.ParentId == folderId)
            .ToListAsync();

        foreach (var folder in childFolders)
        {
            await RemoveFolderWithContentAsync(folder.Id);
        }

        var assets = await _assetsContext.Assets.AsNoTracking()
            .Where(asset => asset.FolderId == folderId)
            .ToListAsync();

        foreach (var asset in assets)
        {
            await _assetsService.RemoveAssetAsync(asset.Id);
        }
    }
}