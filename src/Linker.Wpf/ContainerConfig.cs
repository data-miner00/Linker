namespace Linker.Wpf
{
    using System.Data;
    using System.Data.SQLite;
    using Autofac;
    using Autofac.Configuration;
    using Linker.Core.Repositories;
    using Linker.Data.SQLite;
    using Linker.Wpf.Options;
    using Microsoft.Extensions.Configuration;

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

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.json");

            var config = configBuilder.Build();

            var module = new ConfigurationModule(configBuilder.Build());

            builder.RegisterModule(module);

            var sqliteOptions = config.GetSection(typeof(SQLiteOption).Name).Get<SQLiteOption>();

            var connection = new SQLiteConnection(sqliteOptions.ConnectionString);

            builder.RegisterInstance(connection).As<IDbConnection>();
            builder.RegisterType<ArticleRepository>().As<IArticleRepository>().SingleInstance();
            builder.RegisterType<WebsiteRepository>().As<IWebsiteRepository>().SingleInstance();
            builder.RegisterType<YoutubeRepository>().As<IYoutubeRepository>().SingleInstance();
            builder.RegisterType<MainWindow>().SingleInstance();

            return builder.Build();
        }
    }
}
