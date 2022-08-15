using MamisSolidarias.Infrastructure.Users.Models;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace MamisSolidarias.Infrastructure.Users;

internal class UsersDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new RoleTypeConfiguration().Configure(modelBuilder.Entity<Role>());
        new UserTypeConfiguration().Configure(modelBuilder.Entity<User>());
    }
}