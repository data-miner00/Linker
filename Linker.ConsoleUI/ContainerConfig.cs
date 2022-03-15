using Autofac;
using Linker.ConsoleUI.Controllers;
using Linker.ConsoleUI.Repositories;
using Linker.Core.Repositories;
using Linker.Core.Controllers;

namespace Linker.ConsoleUI
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Startup>().As<IStartup>();
            builder.RegisterType<LinkController>().As<ILinkController>();
            builder.RegisterType<CsvLinkRepository>().As<ILinkRepository>();


            //builder.RegisterAssemblyTypes(Assembly.Load(nameof(DemoLibray)))
            //    .Where(t => t.Namespace.Contains("ABC"))
            //    .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            return builder.Build();
        }
    }
}
