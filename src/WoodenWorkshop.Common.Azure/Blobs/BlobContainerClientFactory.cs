using Azure.Storage.Blobs;

using WoodenWorkshop.Common.Azure.Blobs.Abstractions;

namespace WoodenWorkshop.Common.Azure.Blobs;

public class BlobContainerClientFactory : IBlobContainerClientFactory
{
    private readonly string _connectionString;

    public BlobContainerClientFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public BlobContainerClient CreateBlobContainerClient(string containerName)
    {
        return new BlobContainerClient(_connectionString, containerName);
    }
}