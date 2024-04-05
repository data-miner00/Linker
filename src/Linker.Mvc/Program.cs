using System.Data;
using System.Data.SqlClient;
using AutoMapper;
using Linker.Core.V2;
using Linker.Core.V2.Repositories;
using Linker.Data.SqlServer;
using Linker.Mvc;
using Linker.Mvc.Mappers;
using Linker.Mvc.Options;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

var databaseConnectionString = builder.Configuration.GetConnectionString("Database");

using var connection = new SqlConnection(databaseConnectionString);

var environment = builder.Environment;

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json");

var fileUploadOption = new FileUploadOption();
builder.Configuration.GetSection(nameof(FileUploadOption)).Bind(fileUploadOption);

var cookieOption = builder.Configuration.GetSection(nameof(CookieOption)).Get<CookieOption>();
var credentialOption = builder.Configuration.GetSection(nameof(CredentialOption)).Get<CredentialOption>();

if (credentialOption is null || cookieOption is null)
{
    Console.WriteLine("Credential or Cookie settings is not configured properly.");
    return;
}

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = cookieOption.LoginPath;
        option.ExpireTimeSpan = TimeSpan.FromMinutes(cookieOption.TimeoutInMinutes);
    });

builder.Services
    .AddSingleton<IDbConnection>(connection)
    .AddSingleton<ILinkRepository, LinkRepository>()
    .AddSingleton<IUserRepository, UserRepository>()
    .AddSingleton<IWorkspaceRepository, WorkspaceRepository>()
    .AddScoped<ICredentialRepository, CredentialRepository>()
    .AddScoped<IAuthenticationHandler>(
        ctx => new AuthenticationHandler(
            ctx.GetRequiredService<ICredentialRepository>(), credentialOption.SaltLength))
    .AddSingleton<IAssetUploader>(
        ctx => new FileSystemAssetUploader(
            fileUploadOption.PhysicalUploadOption.BasePath,
            fileUploadOption.PhysicalUploadOption.FolderName,
            fileUploadOption.PhysicalUploadOption.AllowedExtensions));

builder.Services
    .AddSingleton(new MapperConfiguration(config =>
    {
        config.AllowNullCollections = false;
        config.AddProfile<LinkMapperProfile>();
        config.AddProfile<UserMapperProfile>();
        config.AddProfile<WorkspaceMapperProfile>();
    }).CreateMapper());

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
