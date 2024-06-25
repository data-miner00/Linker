namespace Linker.Mvc;

using System.Data;
using System.Data.SqlClient;
using System.Net;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using AutoMapper;
using Linker.Core.V2;
using Linker.Core.V2.Repositories;
using Linker.Data.SqlServer;
using Linker.Mvc.Hubs;
using Linker.Mvc.Mappers;
using Linker.Mvc.Options;
using Linker.Mvc.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

using AppMetrics = Microsoft.Extensions.DependencyInjection.AppMetricsServiceCollectionExtensions;

/// <summary>
/// The entry point for Linker Mvc.
/// </summary>
public static class Program
{
    /// <summary>
    /// Configures all necessary dependencies and start the service.
    /// </summary>
    /// <param name="args">The optional arguments.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddHttpContextAccessor();

        builder
            .ConfigureOptions()
            .ConfigureDatabase()
            .ConfigureRepositories()
            .ConfigureAuths()
            .ConfigureMappers()
            .ConfigureChat()
            .ConfigureAssetsUploader()
            .ConfigureLoggings()
            .ConfigureHttpClients()
            .ConfigureMetrics()
            .Start();
    }

    private static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder builder)
    {
        var environment = builder.Environment;

        builder.Configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json");

        var fileUploadOption = new FileUploadOption();
        builder.Configuration.GetSection(nameof(FileUploadOption)).Bind(fileUploadOption);

        var cookieOption = builder.Configuration.GetSection(nameof(CookieOption)).Get<CookieOption>();
        var credentialOption = builder.Configuration.GetSection(nameof(CredentialOption)).Get<CredentialOption>();

        if (cookieOption is null || credentialOption is null)
        {
            throw new InvalidOperationException("CookieOption and CredentialOption must not be null.");
        }

        builder.Services.AddSingleton(fileUploadOption);
        builder.Services.AddSingleton(cookieOption);
        builder.Services.AddSingleton(credentialOption);

        return builder;
    }

    private static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        var databaseConnectionString = builder.Configuration.GetConnectionString("Database");

        var connection = new SqlConnection(databaseConnectionString);

        builder.Services.AddSingleton<IDbConnection>(connection);

        return builder;
    }

    private static WebApplicationBuilder ConfigureRepositories(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddSingleton<ILinkRepository, LinkRepository>()
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<IWorkspaceRepository, WorkspaceRepository>()
            .AddSingleton<IPlaylistRepository, PlaylistRepository>();

        return builder;
    }

    private static WebApplicationBuilder ConfigureAuths(this WebApplicationBuilder builder)
    {
        var cookieOption = builder.Configuration.GetSection(nameof(CookieOption)).Get<CookieOption>();
        var credentialOption = builder.Configuration.GetSection(nameof(CredentialOption)).Get<CredentialOption>();

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(option =>
            {
                option.LoginPath = cookieOption.LoginPath;
                option.ExpireTimeSpan = TimeSpan.FromMinutes(cookieOption.TimeoutInMinutes);
            });

        builder.Services
            .AddScoped<ICredentialRepository, CredentialRepository>()
            .AddScoped<IAuthenticationHandler>(
                ctx => new AuthenticationHandler(
                    ctx.GetRequiredService<ICredentialRepository>(), credentialOption.SaltLength));

        return builder;
    }

    private static WebApplicationBuilder ConfigureMappers(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddSingleton(new MapperConfiguration(config =>
            {
                config.AllowNullCollections = false;
                config.AddProfile<LinkMapperProfile>();
                config.AddProfile<UserMapperProfile>();
                config.AddProfile<WorkspaceMapperProfile>();
            }).CreateMapper());

        return builder;
    }

    private static WebApplicationBuilder ConfigureChat(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<ConnectionManager>();

        return builder;
    }

    private static WebApplicationBuilder ConfigureAssetsUploader(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IChatRepository, ChatRepository>();

        builder.Services
            .AddSingleton<IAssetUploader>(
                ctx =>
                {
                    var fileUploadOption = ctx.GetRequiredService<FileUploadOption>();
                    return new FileSystemAssetUploader(
                        fileUploadOption.PhysicalUploadOption.BasePath,
                        fileUploadOption.PhysicalUploadOption.FolderName,
                        fileUploadOption.PhysicalUploadOption.AllowedExtensions);
                });

        return builder;
    }

    private static WebApplicationBuilder ConfigureLoggings(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        return builder;
    }

    private static WebApplicationBuilder ConfigureHttpClients(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("github", ConfigureClient);
        builder.Services.AddHttpClient<GitHubService>(ConfigureClient);

        return builder;

        static void ConfigureClient(IServiceProvider ctx, HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", string.Empty);
            httpClient.DefaultRequestHeaders.Add("User-Agent", string.Empty);
            httpClient.BaseAddress = new Uri("https://api.github.com");
        }
    }

    private static WebApplicationBuilder ConfigureMetrics(this WebApplicationBuilder builder)
    {
        AppMetrics.AddMetrics(builder.Services);

        builder.Host
            .UseMetricsWebTracking()
            .UseMetrics(opt =>
            {
                opt.EndpointOptions = (eopt) =>
                {
                    eopt.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                    eopt.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                    eopt.EnvironmentInfoEndpointEnabled = false;
                };
            });

        return builder;
    }

    private static void Start(this WebApplicationBuilder builder)
    {
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.Use(async (context, next) =>
        {
            await next();

            if (context.Response.StatusCode.Equals((int)HttpStatusCode.NotFound))
            {
                context.Request.Path = "/Home/NotFound";
                await next();
            }
        });

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Auth}/{action=Login}/{id?}");

        app.MapHub<ChatHub>("/Chat");

        app.Run();
    }
}
