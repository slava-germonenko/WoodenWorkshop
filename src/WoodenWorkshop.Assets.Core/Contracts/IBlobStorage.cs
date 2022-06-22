namespace WoodenWorkshop.Assets.Core.Contracts;

public interface IBlobStorage
{
    public Task<Uri> UploadBlobAsync(string name, Stream content);

    public Task ReplaceBlobAsync(Uri uri, Stream content);

    public Task RemoveBlobAsync(Uri uri);
}