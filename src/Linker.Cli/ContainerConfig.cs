namespace Linker.Cli;

using Autofac;
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
        builder
            .RegisterType<CsvSerializer<Link>>()
            .As<ISerializer<Link>>()
            .SingleInstance();

        return builder;
    }

    private static ContainerBuilder RegisterCommandHandlers(this ContainerBuilder builder)
    {
        builder.Register(ctx =>
        {
            var linkRepo = ctx.Resolve<IRepository<Link>>();
            return new Lazy<AddLinkCommandHandler>(() => new AddLinkCommandHandler(linkRepo));
        });

        builder.Register(ctx =>
        {
            var linkRepo = ctx.Resolve<ILinkRepository>();
            return new Lazy<ShowLinksCommandHandler>(() => new ShowLinksCommandHandler(linkRepo));
        });

        builder.Register(ctx =>
        {
            var linkRepo = ctx.Resolve<IRepository<Link>>();
            return new Lazy<UpdateLinkCommandHandler>(() => new UpdateLinkCommandHandler(linkRepo));
        });

        builder.Register(ctx =>
        {
            var linkRepo = ctx.Resolve<IRepository<Link>>();
            return new Lazy<DeleteLinkCommandHandler>(() => new DeleteLinkCommandHandler(linkRepo));
        });

        builder.Register(ctx =>
        {
            var linkRepo = ctx.Resolve<IRepository<Link>>();
            var visitRepo = ctx.Resolve<IRepository<Visit>>();
            return new Lazy<VisitLinkCommandHandler>(() => new VisitLinkCommandHandler(linkRepo, visitRepo));
        });

        builder.Register(ctx =>
        {
            var listRepo = ctx.Resolve<IRepository<UrlList>>();
            return new Lazy<CreateListCommandHandler>(() => new CreateListCommandHandler(listRepo));
        });

        builder.Register(ctx =>
        {
            var listRepo = ctx.Resolve<IRepository<UrlList>>();
            return new Lazy<ShowListsCommandHandler>(() => new ShowListsCommandHandler(listRepo));
        });

        builder.Register(ctx =>
        {
            var listRepo = ctx.Resolve<IRepository<UrlList>>();
            return new Lazy<UpdateListCommandHandler>(() => new UpdateListCommandHandler(listRepo));
        });

        builder.Register(ctx =>
        {
            var listRepo = ctx.Resolve<IRepository<UrlList>>();
            return new Lazy<DeleteListCommandHandler>(() => new DeleteListCommandHandler(listRepo));
        });

        builder.Register(ctx =>
        {
            var listRepo = ctx.Resolve<IRepository<UrlList>>();
            var linkRepo = ctx.Resolve<IRepository<Link>>();
            var dbContext = ctx.Resolve<AppDbContext>();
            return new Lazy<AddLinkIntoListCommandHandler>(() => new AddLinkIntoListCommandHandler(listRepo, linkRepo, dbContext));
        });

        builder.Register(ctx =>
        {
            var listRepo = ctx.Resolve<IRepository<UrlList>>();
            var linkRepo = ctx.Resolve<IRepository<Link>>();
            var dbContext = ctx.Resolve<AppDbContext>();
            return new Lazy<RemoveLinkFromListCommandHandler>(() => new RemoveLinkFromListCommandHandler(listRepo, linkRepo, dbContext));
        });

        builder.Register(ctx =>
        {
            var linkRepo = ctx.Resolve<IRepository<Link>>();
            return new Lazy<SearchLinkCommandHandler>(() => new SearchLinkCommandHandler(linkRepo));
        });

        builder.Register(ctx =>
        {
            var linkRepo = ctx.Resolve<IRepository<Link>>();
            return new Lazy<GetLinkCommandHandler>(() => new GetLinkCommandHandler(linkRepo));
        });

        builder.Register(ctx =>
        {
            var listRepo = ctx.Resolve<IRepository<UrlList>>();
            return new Lazy<GetListCommandHandler>(() => new GetListCommandHandler(listRepo));
        });

        builder.Register(ctx =>
        {
            var linkRepo = ctx.Resolve<IRepository<Link>>();
            var serializer = ctx.Resolve<ISerializer<Link>>();
            return new Lazy<ExportLinksCommandHandler>(() => new ExportLinksCommandHandler(linkRepo, serializer));
        });

        return builder;
    }
}
