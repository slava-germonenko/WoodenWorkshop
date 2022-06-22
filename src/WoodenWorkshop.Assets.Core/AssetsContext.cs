using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Assets.Core.Models;
using WoodenWorkshop.Assets.Core.Models.Configuration;
using WoodenWorkshop.Common.EntityFramework;

namespace WoodenWorkshop.Assets.Core;

public class AssetsContext : BaseDbContext
{
    public DbSet<Asset> Assets => Set<Asset>();

    public DbSet<Folder> Folders => Set<Folder>();

    public AssetsContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AssetsEntityConfiguration());
    }
}