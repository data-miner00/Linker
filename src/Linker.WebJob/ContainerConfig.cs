namespace Linker.WebJob;

using Autofac;
using Autofac.Configuration;
using Linker.Core.V2.Repositories;
using Linker.Data.SqlServer;
using Linker.WebJob.Jobs;
using Linker.WebJob.Models;
using Linker.WebJob.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Data;
using System.Data.SqlClient;

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
            .RegisterDatabaseConnection()
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

        var databaseOption = config
            .GetSection(nameof(DatabaseOption))
            .Get<DatabaseOption>();

        var urlHealthCheckOption = config
            .GetSection(nameof(UrlHealthCheckOption))
            .Get<UrlHealthCheckOption>();

        ArgumentNullException.ThrowIfNull(databaseOption);
        ArgumentNullException.ThrowIfNull(urlHealthCheckOption);

        builder.RegisterInstance(databaseOption);
        builder.RegisterInstance(urlHealthCheckOption);

        return builder;
    }

    private static ContainerBuilder RegisterDatabaseConnection(this ContainerBuilder builder)
    {
        builder
            .Register(ctx =>
            {
                var option = ctx.Resolve<DatabaseOption>();

                var connection = new SqlConnection(option.ConnectionString);

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
        builder.RegisterType<LinkRepository>().As<ILinkRepository>().SingleInstance();
        builder.RegisterType<HealthCheckRepository>().As<IHealthCheckRepository>().SingleInstance();

        return builder;
    }
}
