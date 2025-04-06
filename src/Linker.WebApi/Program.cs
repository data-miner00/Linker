using System.Data;
using System.Data.SQLite;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using AutoMapper;
using Linker.Core.Repositories;
using Linker.Data.SQLite;
using Linker.WebApi.Exceptions;
using Linker.WebApi.Filters;
using Linker.WebApi.HealthChecks;
using Linker.WebApi.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
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

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(pb =>
    {
        pb.AllowAnyOrigin();
        pb.AllowAnyMethod();
        pb.AllowAnyHeader();
    });
});

builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

builder.Services.AddOpenTelemetry()
    .WithMetrics(x =>
    {
        x.AddPrometheusExporter();
        x.AddMeter(
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel",
            "Linker");
    });

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

builder.Services.AddRequestTimeouts(opt =>
{
    opt.DefaultPolicy = new RequestTimeoutPolicy
    {
        Timeout = TimeSpan.FromSeconds(5),
        TimeoutStatusCode = (int)HttpStatusCode.RequestTimeout,
    };

    opt.AddPolicy("MoreThanTenSeconds", TimeSpan.FromSeconds(10));
});

builder.Services
    .AddSingleton<IDbConnection>(connection)
    .AddSingleton<IWebsiteRepository, WebsiteRepository>()
    .AddSingleton<IArticleRepository, ArticleRepository>()
    .AddSingleton<IYoutubeRepository, YoutubeRepository>()
    .AddSingleton<ITagRepository, TagRepository>()
    .AddSingleton<IUserRepository, UserRepository>()
    .AddScoped<IWorkspaceRepository, WorkspaceRepository>();

builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, MinimumAgePolicyProvider>();

builder.Services
    .AddSingleton(new MapperConfiguration(config =>
    {
        config.AllowNullCollections = false;
        config.AddProfile<ArticleMapperProfile>();
        config.AddProfile<WebsiteMapperProfile>();
        config.AddProfile<YoutubeMapperProfile>();
        config.AddProfile<UserMapperProfile>();
        config.AddProfile<WorkspaceMapperProfile>();
    })
    .CreateMapper());

builder.Services.AddMetrics();

builder.Services.AddHttpLogging(static opt => opt = new());

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("DatabaseHealthCheck");

builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app
        .UseSwagger()
        .UseSwaggerUI();
}

app
    .UseCors()
    .UseHttpLogging()
    .UseMiddleware<ExceptionHandlerMiddleware>()
    .UseExceptionHandler("/error")
    .UseHttpsRedirection()
    .UseRequestTimeouts()
    .UseAuthentication()
    .UseAuthorization();

app.MapPrometheusScrapingEndpoint();

app.MapControllers();

app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
    },
});

app.Run();
