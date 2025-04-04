﻿using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
	public void Configure(EntityTypeBuilder<Sale> builder)
	{
		builder.ToTable("Sales");

		builder.HasKey(s => s.Id);
		builder.Property(s => s.Id)
			.HasColumnType("uuid")
			.HasDefaultValueSql("gen_random_uuid()");

		builder.Property(s => s.SaleDate).IsRequired();
		builder.Property(s => s.CustomerId).IsRequired().HasColumnType("uuid");
		builder.Property(s => s.BranchId).IsRequired().HasColumnType("uuid");
		builder.Property(s => s.TotalAmount).HasColumnType("decimal(18,2)");
		builder.Property(s => s.IsCancelled).IsRequired();
		
		builder.HasMany(s => s.SaleItems)
			.WithOne(si => si.Sale)
			.HasForeignKey(si => si.SaleId);
	}
}
