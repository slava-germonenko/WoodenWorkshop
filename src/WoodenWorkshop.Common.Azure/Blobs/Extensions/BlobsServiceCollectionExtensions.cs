using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using WoodenWorkshop.Common.Azure.Blobs.Abstractions;

namespace WoodenWorkshop.Common.Azure.Blobs.Extensions;

public static class BlobsServiceCollectionExtensions
{
    public static void AddBlobContainerClientFactory(
        this IServiceCollection services,
        string connectionString,
        ServiceLifetime factoryLifetime = ServiceLifetime.Scoped
    )
    {
        var serviceDescriptor = new ServiceDescriptor(
            typeof(IBlobContainerClientFactory),
            _ => new BlobContainerClientFactory(connectionString),
            factoryLifetime
        );
        
        services.TryAdd(serviceDescriptor);
    }
}