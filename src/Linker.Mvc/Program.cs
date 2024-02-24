using System.Data;
using System.Data.SQLite;
using Linker.Core.Repositories;
using Linker.Data.SQLite;

var builder = WebApplication.CreateBuilder(args);

var sqliteConnectionString = builder.Configuration["SQLiteOption:ConnectionString"];

using var connection = new SQLiteConnection(sqliteConnectionString);
builder.Services.AddControllersWithViews();

builder.Services
    .AddSingleton<IDbConnection>(connection)
    .AddSingleton<IArticleRepository, ArticleRepository>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
