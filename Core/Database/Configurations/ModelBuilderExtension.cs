using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Core.Database.Configurations
{
    internal static class ModelBuilderExtension
    {
        public static void AddEntityConfiguration(this ModelBuilder modelBuilder, Assembly assembly, Type contextType)
        {
            var types = assembly.GetConfigurationTypes(contextType);
            foreach (var config in types.Select(Activator.CreateInstance).Cast<IEntityMappingConfiguration>())
            {
                config.Map(modelBuilder);
            }
        }

        private static IEnumerable<Type> GetConfigurationTypes(this Assembly assembly, Type contextType)
        {
            var contextTypes = contextType.GetProperties().Where(p => p.PropertyType.IsGenericType &&
                p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)).SelectMany(x => x.PropertyType.GetGenericArguments());
            return assembly.GetTypes().Where(x => !x.IsAbstract && x.BaseType.IsGenericType && (x.BaseType.GetGenericTypeDefinition() == typeof(EntityMappingConfiguration<>)
                || x.BaseType.GetGenericTypeDefinition() == typeof(MappingConfiguration<>)) && x.BaseType.GetGenericArguments().Any(x => contextTypes.Contains(x)));
        }
    }
}
