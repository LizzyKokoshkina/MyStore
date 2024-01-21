using Core.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Database.Configurations
{
    internal abstract class EntityMappingConfiguration<T> : MappingConfiguration<T> where T: EntityBase
    {
        public override void Map(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
        }
    }

    internal abstract class MappingConfiguration<T> : IEntityMappingConfiguration<T> where T: class
    {
        public abstract void Map(EntityTypeBuilder<T> builder);

        public void Map(ModelBuilder builder)
        {
            var entityBuilder = builder.Entity<T>();
            Map(entityBuilder);
        }
    }
}
