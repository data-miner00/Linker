namespace Linker.Cli;

using Autofac;
using Linker.Cli.Core;
using Linker.Cli.Handlers;
using Linker.Cli.Integrations;

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

        builder
            .RegisterRepositories()
            .RegisterCommandHandlers();

        builder
            .RegisterType<Application>()
            .AsSelf()
            .SingleInstance();

        return builder.Build();
    }

    private static ContainerBuilder RegisterRepositories(this ContainerBuilder builder)
    {
#if DEBUG
        var connStr = "Data Source=D:\\db.sqlite;";
#else
        var connStr = "Data Source=D:\\prod.sqlite;";
#endif
        var dbContext = new AppDbContext(connStr);

        builder.RegisterInstance(dbContext);

        builder
            .RegisterType<LinkRepository>()
            .As<IRepository<Link>>()
            .SingleInstance();

        builder
            .RegisterType<VisitRepository>()
            .As<IRepository<Visit>>()
            .SingleInstance();

        builder
            .RegisterType<UrlListRepository>()
            .As<IRepository<UrlList>>()
            .SingleInstance();

        return builder;
    }

    private static ContainerBuilder RegisterCommandHandlers(this ContainerBuilder builder)
    {
        builder.RegisterType<AddLinkCommandHandler>().AsSelf().SingleInstance();
        builder.RegisterType<ShowLinksCommandHandler>().AsSelf().SingleInstance();
        builder.RegisterType<UpdateLinkCommandHandler>().AsSelf().SingleInstance();
        builder.RegisterType<DeleteLinkCommandHandler>().AsSelf().SingleInstance();
        builder.RegisterType<VisitLinkCommandHandler>().AsSelf().SingleInstance();
        builder.RegisterType<CreateListCommandHandler>().AsSelf().SingleInstance();
        builder.RegisterType<UpdateListCommandHandler>().AsSelf().SingleInstance();
        builder.RegisterType<ShowListsCommandHandler>().AsSelf().SingleInstance();
        builder.RegisterType<DeleteListCommandHandler>().AsSelf().SingleInstance();
        builder.RegisterType<AddLinkIntoListCommandHandler>().AsSelf().SingleInstance();
        builder.RegisterType<RemoveLinkFromListCommandHandler>().AsSelf().SingleInstance();
        builder.RegisterType<SearchLinkCommandHandler>().AsSelf().SingleInstance();

        return builder;
    }
}
