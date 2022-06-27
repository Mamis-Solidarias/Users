using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace MamisSolidarias.Infrastructure.Users.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    
    public ICollection<Role> Roles { get; set; }
}

internal class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsUnicode()
            .IsRequired();

        builder.Property(t => t.Password)
            .IsRequired();

        builder.Property(t => t.Email)
            .IsUnicode()
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(t => t.Phone)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasMany(t => t.Roles);

    }
} 