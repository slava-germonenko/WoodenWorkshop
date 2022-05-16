using Azure.Storage.Blobs;

namespace WoodenWorkshop.Common.Azure.Blobs.Abstractions;

public interface IBlobContainerClientFactory
{
    public BlobContainerClient CreateBlobContainerClient(string containerName);
}