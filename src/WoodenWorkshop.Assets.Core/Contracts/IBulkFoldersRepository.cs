namespace WoodenWorkshop.Assets.Core.Contracts;

public interface IBulkFoldersRepository
{
    public Task MarkFolderContentAsRemoved(int folderId);
}