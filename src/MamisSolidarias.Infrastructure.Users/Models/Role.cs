using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MamisSolidarias.Infrastructure.Users.Models;

public enum Services
{
    Users
}

public class Role
{
    public int Id { get; set; }
    public Services Service { get; set; }
    public bool CanRead { get; set; }
    public bool CanWrite { get; set; }
}

public class RoleTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();
        
        builder.Property(t => t.Service)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.CanRead)
            .HasDefaultValue(false);

        builder.Property(t => t.CanWrite)
            .HasDefaultValue(false);
    }
}

