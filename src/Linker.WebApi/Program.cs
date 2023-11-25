using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Security.Claims;
using AutoMapper;
using Linker.Core.Repositories;
using Linker.Data.SQLite;
using Linker.WebApi.Mappers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

const string AuthScheme = "cookie";

var builder = WebApplication.CreateBuilder(args);

var sqliteConnectionString = builder.Configuration["SQLite:ConnectionString"];

using var connection = new SQLiteConnection(sqliteConnectionString);

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Linker API", Version = "v1" });
    c.EnableAnnotations();
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

builder.Services.AddAuthentication()
    .AddCookie(AuthScheme);

builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy("minimum_role", pb =>
    {
        pb
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes(AuthScheme)
            .RequireClaim(ClaimTypes.Role, "User");
    });
});

builder.Services
    .AddSingleton<IDbConnection>(
        _ => connection)
    .AddSingleton<IWebsiteRepository, WebsiteRepository>()
    .AddSingleton<IArticleRepository, ArticleRepository>()
    .AddSingleton<IYoutubeRepository, YoutubeRepository>()
    .AddSingleton<ITagRepository, TagRepository>()
    .AddSingleton<IUserRepository, UserRepository>();

builder.Services
    .AddSingleton<IMapper>(c => new MapperConfiguration(config =>
    {
        config.AllowNullCollections = false;
        config.AddProfile<ArticleMapperProfile>();
    })
    .CreateMapper());

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
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();

app.Run();
