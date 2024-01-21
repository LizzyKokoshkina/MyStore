using Core.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Database.Configurations
{
    internal class ProductCategoryConfiguration : MappingConfiguration<ProductCategory>
    {
        public override void Map(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasOne(x => x.Product).WithMany(x => x.Categories).HasForeignKey(x => x.ProductId);
            builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
        }
    }
}
