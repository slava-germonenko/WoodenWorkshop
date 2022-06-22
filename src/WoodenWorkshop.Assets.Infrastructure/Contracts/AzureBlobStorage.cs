using Azure.Storage.Blobs;

using Microsoft.Extensions.Options;

using WoodenWorkshop.Assets.Core.Contracts;
using WoodenWorkshop.Assets.Infrastructure.Options;

namespace WoodenWorkshop.Assets.Infrastructure.Contracts;

public class AzureBlobStorage : IBlobStorage
{
    private readonly BlobContainerClient _blobContainerClient;

    public AzureBlobStorage(BlobServiceClient blobServiceClient, IOptionsSnapshot<StorageOptions> storageOptions)
    {
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(storageOptions.Value.AssetsContainerName);
    }

    public async Task<Uri> UploadBlobAsync(string name, Stream content)
    {
        var blobClient = _blobContainerClient.GetBlobClient(name);
        await blobClient.UploadAsync(content);
        return blobClient.Uri;
    }

    public async Task ReplaceBlobAsync(Uri uri, Stream content)
    {
        var blobClient = new BlobClient(uri);
        await blobClient.UploadAsync(content);
    }

    public Task RemoveBlobAsync(Uri uri)
    {
        var blobClient = new BlobClient(uri);
        return blobClient.DeleteAsync();
    }
}