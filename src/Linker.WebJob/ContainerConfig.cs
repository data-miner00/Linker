namespace Linker.WebJob;

using Autofac;
using Autofac.Configuration;
using Linker.Core.Repositories;
using Linker.Data.SQLite;
using Linker.WebJob.Jobs;
using Linker.WebJob.Models;
using Linker.WebJob.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Data;
using System.Data.SQLite;

/// <summary>
/// The IoC container configurations. Registers all dependencies of the program.
/// </summary>
internal static class ContainerConfig
{
    /// <summary>
    /// Registers the dependencies accordingly.
    /// </summary>
    /// <returns>The IoC container.</returns>
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();

        builder
            .RegisterOptions()
            .RegisterRepositories()
            .RegisterSQLiteConnection()
            .RegisterUrlHealthCheck();

        return builder.Build();
    }

    private static ContainerBuilder RegisterOptions(this ContainerBuilder builder)
    {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddJsonFile("settings.json");

        var config = configBuilder.Build();

        var module = new ConfigurationModule(config);

        builder.RegisterModule(module);

        var sqliteOptions = config
            .GetSection(nameof(SQLiteOption))
            .Get<SQLiteOption>();

        var urlHealthCheckOption = config
            .GetSection(nameof(UrlHealthCheckOption))
            .Get<UrlHealthCheckOption>();

        ArgumentNullException.ThrowIfNull(sqliteOptions);
        ArgumentNullException.ThrowIfNull(urlHealthCheckOption);

        builder.RegisterInstance(sqliteOptions);
        builder.RegisterInstance(urlHealthCheckOption);

        return builder;
    }

    private static ContainerBuilder RegisterSQLiteConnection(this ContainerBuilder builder)
    {
        builder
            .Register(ctx =>
            {
                var option = ctx.Resolve<SQLiteOption>();

                var connection = new SQLiteConnection(option.ConnectionString);

                return connection;
            })
            .As<IDbConnection>()
            .SingleInstance();

        return builder;
    }

    private static ContainerBuilder RegisterUrlHealthCheck(this ContainerBuilder builder)
    {
        builder
            .Register(ctx =>
            {
                var option = ctx.Resolve<UrlHealthCheckOption>();

                var jobDescriptors = new List<JobDescriptor>
                {
                    new()
                    {
                        JobType = typeof(UrlHealthCheckJob),
                        Description = "Regularly checks on the status of url links.",
                        CronExpression = option.CronExpression,
                    },
                };

                return jobDescriptors;
            })
            .As<IEnumerable<JobDescriptor>>()
            .SingleInstance();

        var defaultScheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();

        builder.RegisterInstance(new HttpClient()).SingleInstance();
        builder.RegisterType<JobFactory>().As<IJobFactory>();
        builder.RegisterType<UrlHealthChecker>().As<IUrlHealthChecker>().SingleInstance();
        builder.RegisterType<UrlHealthCheckJob>().SingleInstance();
        builder.RegisterInstance(defaultScheduler).As<IScheduler>().SingleInstance();
        builder.RegisterType<JobScheduler>().SingleInstance();
        builder.RegisterType<WebJobService>().As<IHostedService>().SingleInstance();

        return builder;
    }

    private static ContainerBuilder RegisterRepositories(this ContainerBuilder builder)
    {
        builder.RegisterType<ArticleRepository>().As<IArticleRepository>().SingleInstance();
        builder.RegisterType<WebsiteRepository>().As<IWebsiteRepository>().SingleInstance();
        builder.RegisterType<YoutubeRepository>().As<IYoutubeRepository>().SingleInstance();
        builder.RegisterType<HealthCheckRepository>().As<IHealthCheckRepository>().SingleInstance();

        return builder;
    }
}
