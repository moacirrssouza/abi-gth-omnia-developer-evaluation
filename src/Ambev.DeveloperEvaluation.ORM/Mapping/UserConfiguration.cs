using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Phone).HasMaxLength(20);
        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.UpdatedAt).IsRequired(false);

        builder.Property(u => u.Status)
            .HasConversion(
                v => v.ToString(),
                v => MapStatus(v)
            )
            .HasMaxLength(20);

        builder.Property(u => u.Role)
            .HasConversion(
                v => v.ToString(),
                v => MapRole(v)
            )
            .HasMaxLength(20);

    }

    private static UserRole MapRole(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return UserRole.Customer;

        var v = value.ToLowerInvariant();
        return v switch
        {
            "user" => UserRole.Customer,
            "customer" => UserRole.Customer,
            "admin" => UserRole.Admin,
            "manager" => UserRole.Manager,
            _ => UserRole.Customer
        };
    }

    private static UserStatus MapStatus(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return UserStatus.Unknown;

        var v = value.ToLowerInvariant();
        return v switch
        {
            "active" => UserStatus.Active,
            "inactive" => UserStatus.Inactive,
            "suspended" => UserStatus.Suspended,
            "unknown" => UserStatus.Unknown,
            _ => UserStatus.Unknown
        };
    }
}
