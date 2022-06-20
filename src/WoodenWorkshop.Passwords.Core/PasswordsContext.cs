using Microsoft.EntityFrameworkCore;

using WoodenWorkshop.Common.EntityFramework;
using WoodenWorkshop.Passwords.Core.Models;

namespace WoodenWorkshop.Passwords.Core;

public class PasswordsContext : BaseDbContext
{
    public DbSet<User> Users => Set<User>();

    public PasswordsContext(DbContextOptions options) : base(options) { }
}