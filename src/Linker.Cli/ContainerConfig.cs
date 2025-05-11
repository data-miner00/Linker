namespace Linker.Cli;

using Autofac;
using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Core.Serializers;
using Linker.Cli.Handlers;
using Linker.Cli.Integrations;
using Linker.Cli.Integrations.Serializers;

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
            .RegisterSerializers()
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
            .As<ILinkRepository>()
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

    private static ContainerBuilder RegisterSerializers(this ContainerBuilder builder)
    {
        var serializers = new Dictionary<ExportFormat, Lazy<ISerializer<Link>>>
        {
            { ExportFormat.Csv, new Lazy<ISerializer<Link>>(() => new CsvSerializer<Link>()) },
            { ExportFormat.Json, new Lazy<ISerializer<Link>>(() => new JsonSerializer<Link>()) },
        };

        builder
            .Register(ctx => serializers)
            .As<IDictionary<ExportFormat, Lazy<ISerializer<Link>>>>()
            .SingleInstance();

        return builder;
    }

    private static ContainerBuilder RegisterCommandHandlers(this ContainerBuilder builder)
    {
        builder.Register(ctx =>
        {
            var listRepo = ctx.Resolve<IRepository<UrlList>>();
            var linkRepo = ctx.Resolve<IRepository<Link>>();
            var interfaceLinkRepo = ctx.Resolve<ILinkRepository>();
            var serializers = ctx.Resolve<IDictionary<ExportFormat, Lazy<ISerializer<Link>>>>();
            var visitRepo = ctx.Resolve<IRepository<Visit>>();
            var dbContext = ctx.Resolve<AppDbContext>();

            IDictionary<CommandType, Lazy<ICommandHandler>> commandHandlers = new Dictionary<CommandType, Lazy<ICommandHandler>>
            {
                { CommandType.AddLink, new Lazy<ICommandHandler>(() => new AddLinkCommandHandler(linkRepo)) },
                { CommandType.ShowLinks, new Lazy<ICommandHandler>(() => new ShowLinksCommandHandler(interfaceLinkRepo)) },
                { CommandType.UpdateLink, new Lazy<ICommandHandler>(() => new UpdateLinkCommandHandler(linkRepo)) },
                { CommandType.DeleteLink, new Lazy<ICommandHandler>(() => new DeleteLinkCommandHandler(linkRepo)) },
                { CommandType.VisitLink, new Lazy<ICommandHandler>(() => new VisitLinkCommandHandler(linkRepo, visitRepo)) },
                { CommandType.CreateList, new Lazy<ICommandHandler>(() => new CreateListCommandHandler(listRepo)) },
                { CommandType.ShowLists, new Lazy<ICommandHandler>(() => new ShowListsCommandHandler(listRepo)) },
                { CommandType.UpdateList, new Lazy<ICommandHandler>(() => new UpdateListCommandHandler(listRepo)) },
                { CommandType.DeleteList, new Lazy<ICommandHandler>(() => new DeleteListCommandHandler(listRepo)) },
                { CommandType.AddLinkIntoList, new Lazy<ICommandHandler>(() => new AddLinkIntoListCommandHandler(listRepo, linkRepo, dbContext)) },
                { CommandType.RemoveLinkFromList, new Lazy<ICommandHandler>(() => new RemoveLinkFromListCommandHandler(listRepo, linkRepo, dbContext)) },
                { CommandType.SearchLinks, new Lazy<ICommandHandler>(() => new SearchLinkCommandHandler(linkRepo, visitRepo)) },
                { CommandType.GetLink, new Lazy<ICommandHandler>(() => new GetLinkCommandHandler(linkRepo)) },
                { CommandType.GetList, new Lazy<ICommandHandler>(() => new GetListCommandHandler(listRepo)) },
                { CommandType.ExportLinks, new Lazy<ICommandHandler>(() => new ExportLinksCommandHandler(linkRepo, serializers)) },
                { CommandType.SearchLists, new Lazy<ICommandHandler>(() => new SearchListCommandHandler(listRepo)) },
            };

            return commandHandlers;
        });

        return builder;
    }
}
