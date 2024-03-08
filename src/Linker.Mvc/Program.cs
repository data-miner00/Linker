using System.Data;
using System.Data.SQLite;
using AutoMapper;
using Linker.Common.Mappers;
using Linker.Core.Repositories;
using Linker.Data.SQLite;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

var sqliteConnectionString = builder.Configuration["SQLiteOption:ConnectionString"];

using var connection = new SQLiteConnection(sqliteConnectionString);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Auth/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services
    .AddSingleton<IDbConnection>(connection)
    .AddSingleton<IArticleRepository, ArticleRepository>()
    .AddSingleton<IWebsiteRepository, WebsiteRepository>()
    .AddSingleton<IYoutubeRepository, YoutubeRepository>()
    .AddSingleton<IUserRepository, UserRepository>()
    .AddSingleton<IWorkspaceRepository, WorkspaceRepository>();

builder.Services
    .AddSingleton(new MapperConfiguration(config =>
    {
        config.AllowNullCollections = false;
        config.AddProfile<ArticleMapperProfile>();
        config.AddProfile<WebsiteMapperProfile>();
        config.AddProfile<YoutubeMapperProfile>();
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
