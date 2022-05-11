using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.Common.EntityFramework;

public class BaseDbContext : DbContext
{
    public BaseDbContext(DbContextOptions options) : base(options) { }

    public override int SaveChanges()
    {
        SetCreatedDateOnCratedEntities();
        SetUpdatedDateOnUpdatedEntries();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        SetCreatedDateOnCratedEntities();
        SetUpdatedDateOnUpdatedEntries();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new())
    {
        SetCreatedDateOnCratedEntities();
        SetUpdatedDateOnUpdatedEntries();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void SetCreatedDateOnCratedEntities()
    {
        var updatedEntries = ChangeTracker.Entries<BaseModel>().Where(entry => entry.State == EntityState.Added);
        foreach (var updatedEntry in updatedEntries)
        {
            updatedEntry.Entity.UpdatedDate = null;
            updatedEntry.Entity.CreatedDate = DateTime.UtcNow;
        }
    }

    private void SetUpdatedDateOnUpdatedEntries()
    {
        var updatedEntries = ChangeTracker.Entries<BaseModel>().Where(entry => entry.State == EntityState.Modified);
        foreach (var updatedEntry in updatedEntries)
        {
            updatedEntry.Property(entity => entity.CreatedDate).IsModified = false;
            updatedEntry.Entity.UpdatedDate = DateTime.UtcNow;
        }
    }
}