using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Assets.Core.Contracts;
using WoodenWorkshop.Assets.Core.Dtos;
using WoodenWorkshop.Assets.Core.Errors;
using WoodenWorkshop.Assets.Core.Extensions;
using WoodenWorkshop.Assets.Core.Models;
using WoodenWorkshop.Common.Core.Exceptions;
using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.Common.EntityFramework.Extensions;

namespace WoodenWorkshop.Assets.Core;

public class AssetsService
{
    private readonly AssetsContext _context;

    private readonly IBlobStorage _blobStorage;

    public AssetsService(AssetsContext context, IBlobStorage blobStorage)
    {
        _context = context;
        _blobStorage = blobStorage;
    }

    public Task<PagedResult<Asset>> GetAssetsAsync(AssetsFilter filter, AssetsOrder order)
    {
        return _context.Assets.AsNoTracking()
            .ApplyFilters(filter)
            .ApplyOrder(order)
            .ToPagedResultAsync(filter);
    }

    public async Task<Asset> AddAssetAsync(AddAssetDto assetDto)
    {
        await EnsureAssetNameIsNotInUse(assetDto.Name, assetDto.FolderId, null);

        var blobUri = await _blobStorage.UploadBlobAsync(assetDto.Name, assetDto.Blob);
        var asset = new Asset
        {
            Name = assetDto.Name,
            BlobUri = blobUri,
            FolderId = assetDto.FolderId
        };

        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();

        return asset;
    }

    public async Task<Asset> UpdateAssetDetailsAsync(AssetDetailsDto assetDetailsDto)
    {
        var asset = await _context.Assets.FindAsync(assetDetailsDto.AssetId);
        if (asset is null)
        {
            throw new CoreLogicException(ErrorMessages.AssetNotFound, ErrorCodes.AssetNotFound);
        }

        await EnsureAssetNameIsNotInUse(assetDetailsDto.Name, assetDetailsDto.FolderId, assetDetailsDto.AssetId);
        
        asset.Name = assetDetailsDto.Name;
        asset.FolderId = assetDetailsDto.FolderId;

        _context.Update(asset);
        await _context.SaveChangesAsync();

        return asset;
    }

    public async Task<Asset> ReplaceAssetContentAsync(int assetId, Stream blob)
    {
        var asset = await _context.Assets.FindAsync(assetId);
        if (asset is null)
        {
            throw new CoreLogicException(ErrorMessages.AssetNotFound, ErrorCodes.AssetNotFound);
        }

        if (asset.BlobUri is null)
        {
            asset.BlobUri = await _blobStorage.UploadBlobAsync(asset.Name, blob);
        }
        else
        {
            await _blobStorage.ReplaceBlobAsync(asset.BlobUri, blob);
            _context.Entry(asset).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();
        return asset;
    }

    public async Task RemoveAssetAsync(int assetId)
    {
        var asset = await _context.Assets.FindAsync(assetId);
        if (asset is null)
        {
            throw new CoreLogicException(ErrorMessages.AssetNotFound, ErrorCodes.AssetNotFound);
        }

        if (asset.BlobUri is not null)
        {
            await _blobStorage.RemoveBlobAsync(asset.BlobUri);   
        }

        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();
    }

    private async Task EnsureAssetNameIsNotInUse(string name, int? folderId, int? assetId)
    {
        var assetNameInUse = await _context.Assets.AnyAsync(
            asset => asset.Name.Equals(name) 
                     && asset.FolderId == folderId 
                     && asset.DeletedDate == null
                     && asset.Id != assetId
            );

        if (assetNameInUse)
        {
            throw new CoreLogicException(ErrorMessages.AssetNameInUse, ErrorCodes.AssetNameInUse);
        }
    }
}