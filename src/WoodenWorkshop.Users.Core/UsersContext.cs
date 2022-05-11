using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Common.EntityFramework;
using WoodenWorkshop.Users.Core.Models;

namespace WoodenWorkshop.Users.Core;

public class UsersContext : BaseDbContext
{
    public DbSet<User> Users => Set<User>();

    public UsersContext(DbContextOptions options) : base(options) { }
}