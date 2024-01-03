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
    using Linker.Core.Models;
    using Linker.Data.SQLite;

    internal static class Program
    {
        public static void Main(string[] args)
        {
            using (var connection = new SQLiteConnection(LoadConnectionString()))
            {
                var tagRepo = new TagRepository(connection);
                goto some;
            some:
                var tags = tagRepo.GetAllAsync().GetAwaiter().GetResult();

                Console.WriteLine(tags);
            }
        }

        public static async Task AddArticle(IDbConnection connection)
        {
            var articleRepo = new ArticleRepository(connection);

            await articleRepo.AddAsync(
                new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Url = "sample.com/article.htm",
                    Category = Category.Finance,
                    Description = "long desc",
                    Tags = new List<string> { "tag1", "tag2", "new TAG!!!" },
                    Language = Language.Japanese,
                    LastVisitAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    Title = "Crummy title",
                    Author = "Jim Cramer",
                    Year = 2014,
                    WatchLater = true,
                    Domain = "www.google.com",
                    Grammar = Grammar.Average,
                },
                CancellationToken.None);

            await articleRepo.GetAllAsync(CancellationToken.None);
        }

        public static async Task AddYoutube(IDbConnection connection)
        {
            var youtubeRepo = new YoutubeRepository(connection);
            await youtubeRepo.UpdateAsync(
                new Youtube
                {
                    Id = "ae58d402-b7a4-4e69-97d6-30ebcad741c5",
                    Url = "youtube.com/dahuhffd",
                    Category = Category.Finance,
                    Description = "A C# professional guru",
                    Tags = new List<string> { "c#", "unity", "f#" },
                    Language = Language.English,
                    LastVisitAt = new DateTime(1994, 4, 5, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2022, 4, 5, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = DateTime.Now,
                    Name = "Brackeys",
                    Youtuber = "Johnson",
                    Country = "Canada",
                },
                CancellationToken.None);

            var all = await youtubeRepo.GetAllAsync(CancellationToken.None);
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
