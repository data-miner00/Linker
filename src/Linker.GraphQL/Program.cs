using System.Data;
using System.Data.SqlClient;
using AutoMapper;
using Linker.Core.V2.Repositories;
using Linker.Data.SqlServer;
using Linker.GraphQL;
using Linker.GraphQL.Mappers;

{
    var builder = WebApplication.CreateBuilder(args);
    var databaseConnectionString = builder.Configuration.GetConnectionString("Database");
    using var connection = new SqlConnection(databaseConnectionString);

    builder.Services.AddSingleton<IDbConnection>(connection);
    builder.Services.AddSingleton<ILinkRepository, LinkRepository>();
    builder.Services
        .AddSingleton(c => new MapperConfiguration(config =>
        {
            config.AllowNullCollections = false;
            config.AddProfile<LinkMapperProfile>();
            config.AddProfile<UserMapperProfile>();
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
