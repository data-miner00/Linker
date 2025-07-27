namespace Linker.Cli;

using Autofac;
using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Core.Serializers;
using Linker.Cli.Handlers;
using Linker.Cli.Integrations;
using Linker.Cli.Integrations.Serializers;
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

        builder
            .ConfigureSettings()
            .RegisterRepositories()
            .RegisterSerializers()
            .RegisterCommandHandlers();

        builder
            .RegisterType<Application>()
            .AsSelf()
            .SingleInstance();

        return builder.Build();
    }

    private static ContainerBuilder ConfigureSettings(this ContainerBuilder builder)
    {
        var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("settings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"settings.{environment}.json", optional: false, reloadOnChange: false)
            .Build();

        builder.RegisterInstance(configuration).As<IConfiguration>().SingleInstance();

        return builder;
    }

    private static ContainerBuilder RegisterRepositories(this ContainerBuilder builder)
    {
        builder.Register(ctx =>
        {
            var config = ctx.Resolve<IConfiguration>();

            var connStr = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

            return new AppDbContext(connStr);
        }).SingleInstance();

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
        var linkSerializers = new Dictionary<ExportFormat, Lazy<ISerializer<Link>>>
        {
            { ExportFormat.Csv, new Lazy<ISerializer<Link>>(() => new CsvSerializer<Link>()) },
            { ExportFormat.Json, new Lazy<ISerializer<Link>>(() => new JsonSerializer<Link>()) },
        };

        var listSerializers = new Dictionary<ExportFormat, Lazy<ISerializer<UrlList>>>
        {
            { ExportFormat.Csv, new Lazy<ISerializer<UrlList>>(() => new CsvSerializer<UrlList>()) },
            { ExportFormat.Json, new Lazy<ISerializer<UrlList>>(() => new JsonSerializer<UrlList>()) },
        };

        builder
            .Register(ctx => linkSerializers)
            .As<IDictionary<ExportFormat, Lazy<ISerializer<Link>>>>()
            .SingleInstance();

        builder.
            Register(ctx => listSerializers)
            .As<IDictionary<ExportFormat, Lazy<ISerializer<UrlList>>>>()
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
            var linkSerializers = ctx.Resolve<IDictionary<ExportFormat, Lazy<ISerializer<Link>>>>();
            var listSerializers = ctx.Resolve<IDictionary<ExportFormat, Lazy<ISerializer<UrlList>>>>();
            var visitRepo = ctx.Resolve<IRepository<Visit>>();
            var dbContext = ctx.Resolve<AppDbContext>();
            var config = ctx.Resolve<IConfiguration>();

            var linksDefaultExportPath = config.GetValue<string>("DefaultExportPath:Links")
                ?? throw new InvalidOperationException("Links default export path is not configured.");
            var listsDefaultExportPath = config.GetValue<string>("DefaultExportPath:Lists")
                ?? throw new InvalidOperationException("Lists default export path is not configured.");

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
                { CommandType.ExportLinks, new Lazy<ICommandHandler>(() => new ExportLinksCommandHandler(linkRepo, linkSerializers, linksDefaultExportPath)) },
                { CommandType.SearchLists, new Lazy<ICommandHandler>(() => new SearchListCommandHandler(listRepo)) },
                { CommandType.ExportLists, new Lazy<ICommandHandler>(() => new ExportListsCommandHandler(listRepo, linkSerializers, listSerializers, listsDefaultExportPath)) },
            };

            return commandHandlers;
        });

        return builder;
    }
}
