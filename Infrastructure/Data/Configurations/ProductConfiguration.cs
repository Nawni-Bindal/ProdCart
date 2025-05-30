using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ProductName).IsRequired().HasMaxLength(255);
            builder.Property(p => p.CreatedBy).IsRequired().HasMaxLength(100);
            builder.Property(p => p.CreatedOn).IsRequired();
            builder.Property(p => p.ModifiedBy).HasMaxLength(100);
            builder.Property(p => p.ModifiedOn).IsRequired(false);
            builder.HasMany(p => p.Items).WithOne(i => i.Product).HasForeignKey(i => i.ProductId);
        }
    }
}