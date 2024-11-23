namespace Linker.Debugger
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SQLite;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Timers;
    using Dapper;
    using Linker.Core.V2.Models;
    using Linker.Data.SQLite;
    using Linker.Data.SqlServer;
    using Microsoft.Data.SqlClient;

    internal static class Program
    {
        public static void Main(string[] args)
        {
            var connectionString = LoadConnectionString("SqlServer");
            using var connection = new SqlConnection(connectionString);

            var a = GetAll(connection).GetAwaiter().GetResult();
            Console.WriteLine();
        }

        public static Task<IEnumerable<Link>> GetAll(IDbConnection connection)
        {
            var linkRepo = new LinkRepository(connection);
            return linkRepo.GetAllAsync(default);
        }

        public static Task AddLink(IDbConnection connection)
        {
            var linkRepo = new LinkRepository(connection);

            var link = new Link
            {
                Id = Guid.NewGuid().ToString(),
                Url = "sample.com/article.htm",
                Category = Category.Finance,
                Description = "long desc",
                Tags = new List<string> { "tag1", "tag2", "new TAG!!!" },
                Language = Language.Japanese,
                Type = LinkType.Website,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                Name = "Crummy title",
                KeyPersonName = "Jim Cramer",
                Visibility = Visibility.Private,
                Domain = "www.google.com",
                Grammar = Grammar.Average,
                Rating = Rating.PG13,
            };

            return linkRepo.AddAsync(link, default);
        }

        private static string LoadConnectionString(string id = "Default")
        {
            goto next;
            next: return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
