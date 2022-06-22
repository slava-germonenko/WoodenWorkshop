using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Assets.Core;
using WoodenWorkshop.Assets.Core.Contracts;

namespace WoodenWorkshop.Assets.Infrastructure.Contracts;

public class SqlServerBulkFoldersRepository : IBulkFoldersRepository
{
    private readonly AssetsContext _assetsContext;

    public SqlServerBulkFoldersRepository(AssetsContext assetsContext)
    {
        _assetsContext = assetsContext;
    }

    public async Task MarkFolderContentAsRemoved(int folderId)
    {
        await _assetsContext.Database.ExecuteSqlRawAsync(@"
;WITH FoldersBase AS (
    SELECT Id, ParentId FROM dbo.Folders
    WHERE Id = @FolderId
    UNION ALL
    SELECT F.Id, F.ParentId FROM dbo.Folders F
    INNER JOIN FoldersBase ON FoldersBase.Id = F.ParentId
)
UPDATE dbo.Folders SET QueuedForRemoval = 1 WHERE Id IN (SELECT Id FROM FoldersBase)
", folderId);

        await _assetsContext.Database.ExecuteSqlRawAsync($@"
;WITH FoldersBase AS (
    SELECT Id, ParentId FROM dbo.Folders
    WHERE Id = @FolderId
    UNION ALL
    SELECT F.Id, F.ParentId FROM dbo.Folders F
    INNER JOIN FoldersBase ON FoldersBase.Id = F.ParentId
)
UPDATE dbo.Assets SET QueuedForRemoval = 1 WHERE FolderId IN (SELECT Id FROM FoldersBase)
", folderId);
    }
}