using Autofac;
using Core.Entites;

namespace Core.Extensions
{
    public static class EntityExtensions
    {
        public static void CleanNavigationProperties<TEntity>(this TEntity entity) where TEntity : class, ISqlEntity
        {
            var properties = entity.GetType().GetProperties().Where(i => i.PropertyType.IsAssignableTo<ISqlEntity>() ||
                i.PropertyType.IsAssignableTo<IEnumerable<ISqlEntity>>());
            foreach ( var property in properties)
            {
                property.SetValue(entity, null);
            }
        }
    }
}
