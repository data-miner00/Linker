﻿namespace Linker.ConsoleUI
{
    using Autofac;
    using Linker.ConsoleUI.Controllers;
    using Linker.ConsoleUI.Repositories;
    using Linker.ConsoleUI.UI;
    using Linker.Core.Controllers;
    using Linker.Core.Repositories;

    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Startup>().As<IStartup>();
            builder.RegisterType<LinkController>().As<ILinkController>();
            builder.RegisterType<CsvLinkRepository>().As<ILinkRepository>();
            builder.RegisterType<Router>().As<IRouter>();

            // UI
            builder.RegisterType<Menu>().As<IMenu>();


            //builder.RegisterAssemblyTypes(Assembly.Load(nameof(DemoLibray)))
            //    .Where(t => t.Namespace.Contains("ABC"))
            //    .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            return builder.Build();
        }
    }
}
