using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.Date).IsRequired();
        builder.Property(s => s.Customer).HasMaxLength(200);
        builder.Property(s => s.Branch).HasMaxLength(200);

        builder.Property(s => s.TotalAmount).HasColumnType("numeric(18,2)");

        builder.HasMany(s => s.Items)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}