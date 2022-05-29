using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Common.EntityFramework;
using WoodenWorkshop.Invitations.Core.Models;

namespace WoodenWorkshop.Invitations.Core;

public class InvitationsContext : BaseDbContext
{
    public DbSet<Invitation> Invitations => Set<Invitation>();

    public InvitationsContext(DbContextOptions options) : base(options) { }
}