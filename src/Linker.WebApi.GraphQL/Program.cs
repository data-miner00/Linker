using System.Data;
using System.Data.SQLite;
using AutoMapper;
using Linker.Core.Repositories;
using Linker.Data.SQLite;
using Linker.WebApi.GraphQL;
using Linker.WebApi.Mappers;

{
    var builder = WebApplication.CreateBuilder(args);
    var sqliteConnectionString = builder.Configuration["SQLiteOption:ConnectionString"];
    using var connection = new SQLiteConnection(sqliteConnectionString);

    builder.Services.AddSingleton<IDbConnection>(connection);
    builder.Services.AddSingleton<IArticleRepository, ArticleRepository>();
    builder.Services
        .AddSingleton<IMapper>(c => new MapperConfiguration(config =>
        {
            config.AllowNullCollections = false;
            config.AddProfile<ArticleMapperProfile>();
            config.AddProfile<WebsiteMapperProfile>();
            config.AddProfile<YoutubeMapperProfile>();
            config.AddProfile<WorkspaceMapperProfile>();
        })
        .CreateMapper());

    builder.Services
        .AddGraphQLServer()
        .AddQueryType<Query>()
        .AddMutationType<Mutation>();

    var app = builder.Build();

    app.UseHttpsRedirection();

    app.MapGraphQL();

    app.Run();
}
