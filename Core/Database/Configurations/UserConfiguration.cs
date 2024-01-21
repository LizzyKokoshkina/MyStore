using Core.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Database.Configurations
{
    internal class UserConfiguration : EntityMappingConfiguration<User>
    {
        public override void Map(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Firstname).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Lastname).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(128).IsRequired();
            base.Map(builder);
        }
    }
}
