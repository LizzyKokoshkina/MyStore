using Autofac;
using Core.Database.Context;
using Core.Database.Repository;
using Core.Database.UnitOfWork;
using Core.Entites;
using Core.Payments;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MySqlDatabase>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterTypes(GetRepositories()).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<BraintreeConfiguration>().AsImplementedInterfaces().InstancePerLifetimeScope();

            base.Load(builder);
        }

        private Type[] GetRepositories()
        {
            var entities = typeof(ISqlEntity).Assembly.GetTypes().Where(i => i.IsClass && !i.IsAbstract && i.IsAssignableTo<ISqlEntity>());
            return entities.Select(i => typeof(MySqlRepository<>).MakeGenericType(i)).ToArray();
        }
    }
}
