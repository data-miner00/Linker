using System.Data;
using System.Data.SQLite;
using System.Reflection;
using Linker.Core.Repositories;
using Linker.Data.SQLite;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

var sqliteConnectionString = builder.Configuration["SQLite:ConnectionString"];

using var connection = new SQLiteConnection(sqliteConnectionString);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Linker API", Version = "v1" });
    c.EnableAnnotations();
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

builder.Services
    .AddSingleton<IDbConnection>(
        _ => connection)
    .AddSingleton<IWebsiteRepository, WebsiteRepository>()
    .AddSingleton<IArticleRepository, ArticleRepository>()
    .AddSingleton<IYoutubeRepository, YoutubeRepository>()
    .AddSingleton<ITagRepository, TagRepository>()
    .AddSingleton<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app
        .UseSwagger()
        .UseSwaggerUI();
}

app
    .UseExceptionHandler("/error")
    .UseHttpsRedirection()
    .UseAuthorization();

app.MapControllers();

app.Run();
