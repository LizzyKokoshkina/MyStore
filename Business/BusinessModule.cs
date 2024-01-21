using Autofac;
using Business.Managers;
using Business.Services;
using Microsoft.Extensions.Configuration;

namespace Business
{
    public class BusinessModule : Module
    {
        private readonly IConfigurationSection section;
        public BusinessModule(IConfigurationSection section)
        {
            this.section = section;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterTypes(GetManagers()).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<EmailService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            base.Load(builder);
        }

        private Type[] GetManagers()
        {
            return ThisAssembly.GetTypes().Where(i => !i.IsAbstract && i.IsClass & i.IsAssignableTo<ManagerBase>()).ToArray();
        }
    }

}
