using System.Data.SQLite;
using Linker.Core.Repositories;
using Linker.Data.SQLite;

var builder = WebApplication.CreateBuilder(args);

var sqliteConnectionString = builder.Configuration["SQLite:ConnectionString"];

using var connection = new SQLiteConnection(sqliteConnectionString);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services
    .AddSingleton<IWebsiteRepository>(
        _ => new WebsiteRepository(connection))
    .AddSingleton<IArticleRepository>(
        _ => new ArticleRepository(connection))
    .AddSingleton<IYoutubeRepository>(
        _ => new YoutubeRepository(connection))
    .AddSingleton<ITagRepository>(
        _ => new TagRepository(connection));

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
