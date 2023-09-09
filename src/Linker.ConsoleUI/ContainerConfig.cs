namespace Linker.ConsoleUI
{
    using Autofac;
    using Linker.ConsoleUI.Controllers;
    using Linker.ConsoleUI.Repositories;
    using Linker.ConsoleUI.UI;
    using Linker.Core.Controllers.ConsoleUI;
    using Linker.Core.Repositories;

    /// <summary>
    /// A static class that contains logics for container registration.
    /// </summary>
    internal static class ContainerConfig
    {
        /// <summary>
        /// The actual method that registers the dependencies into an IoC container.
        /// </summary>
        /// <returns>The <see cref="IContainer"/>.</returns>
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Startup>().As<IStartup>();
            builder.RegisterType<WebsiteController>().As<IWebsiteController>();
            builder.RegisterType<InMemoryCsvWebsiteRepository>().As<ICsvWebsiteRepository>();
            builder.RegisterType<ArticleController>().As<IArticleController>();
            builder.RegisterType<InMemoryCsvArticleRepository>().As<ICsvArticleRepository>();
            builder.RegisterType<YoutubeController>().As<IYoutubeController>();
            builder.RegisterType<InMemoryCsvYoutubeRepository>().As<ICsvYoutubeRepository>();
            builder.RegisterType<Router>().As<IRouter>();

            // UI
            builder.RegisterType<Menu>().As<IMenu>();

            return builder.Build();
        }
    }
}
