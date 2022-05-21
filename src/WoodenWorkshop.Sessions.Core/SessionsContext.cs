using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Common.EntityFramework;
using WoodenWorkshop.Sessions.Core.Models;

namespace WoodenWorkshop.Sessions.Core;

public class SessionsContext : BaseDbContext
{
    public DbSet<UserSession> UserSessions => Set<UserSession>();

    public SessionsContext(DbContextOptions options) : base(options) { }
}