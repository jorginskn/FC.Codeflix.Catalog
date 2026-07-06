using FC.Codeflix.Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FC.Codeflix.Catalog.Infra.Data.EF.Configurations;
internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(Category => Category.Id);
        builder.Property(Category => Category.Name).HasMaxLength(255).IsRequired();
        builder.Property(Category => Category.Description).HasMaxLength(10_000);
        builder.Property(Category => Category.IsActive).IsRequired();
    }
}
