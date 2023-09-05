namespace Linker.WebApi
{
    using System.Data.SQLite;
    using Linker.Data.SQLite;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var sqliteConnectionString = builder.Configuration["SQLite:ConnectionString"];
            using var connection = new SQLiteConnection(sqliteConnectionString);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton(new WebsiteRepository(connection));

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
        }
    }
}
