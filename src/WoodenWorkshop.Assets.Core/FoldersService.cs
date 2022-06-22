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

public class FoldersService
{
    private readonly AssetsContext _assetsContext;

    private readonly IBulkFoldersRepository _bulkFoldersRepository;

    public FoldersService(AssetsContext assetsContext, IBulkFoldersRepository bulkFoldersRepository)
    {
        _assetsContext = assetsContext;
        _bulkFoldersRepository = bulkFoldersRepository;
    }

    public Task<PagedResult<Folder>> GetFoldersAsync(FoldersFilter filter, FoldersOrder order)
    {
        return _assetsContext.Folders.AsNoTracking()
            .ApplyFilters(filter)
            .ApplyOrder(order)
            .ToPagedResultAsync(filter);
    }

    public async Task<Folder> SaveFolderAsync(Folder folder)
    {
        var folderNameIsInUse = await _assetsContext.Folders.AnyAsync(
            f => f.DeletedDate == null
                 && f.Id != folder.Id
                 && f.ParentId == folder.ParentId 
                 && f.Name.Equals(f.Name)
        );

        if (folderNameIsInUse)
        {
            throw new CoreLogicException(ErrorMessages.FolderNameIsInUse, ErrorCodes.FolderNameIsInUse);
        }

        _assetsContext.Folders.Update(folder);
        await _assetsContext.SaveChangesAsync();
        return folder;
    }

    public async Task MarkFolderWithContentAsDeleted(int folderId)
    {
        var folder = await _assetsContext.Folders.FindAsync(folderId);
        if (folder is null)
        {
            throw new CoreLogicException(ErrorMessages.FolderNotFound, ErrorCodes.FolderNotFound);
        }

        await _bulkFoldersRepository.MarkFolderContentAsRemoved(folderId);
        folder.DeletedDate = DateTime.UtcNow;
        _assetsContext.Update(folder);
        await _assetsContext.SaveChangesAsync();
    }
}