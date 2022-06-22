using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WoodenWorkshop.Assets.Core.Models.Configuration;

public class AssetsEntityConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.Property(a => a.BlobUri)
            .HasConversion<string?>(
                uri => uri == null ? null : uri.ToString(),
                uriString => uriString == null ? null : new Uri(uriString)
            );
    }
}