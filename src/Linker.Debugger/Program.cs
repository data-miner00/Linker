namespace Linker.Debugger
{
    using System.Configuration;
    using System.Data.SQLite;
    using Linker.Core.Models;
    using Linker.Data.SQLite;

    internal class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SQLiteConnection(LoadConnectionString()))
            {
                var websiteRepo = new WebsiteRepository(connection);

                var website = new Website
                {
                    Id = "new",
                    Url = "sample.com",
                    Category = Category.Finance,
                    Description = "no desc",
                    Tags = new List<string> { "tag1", "tag2" },
                    Language = Language.Japanese,
                    LastVisitAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    Name = "noname",
                    Domain = "doamin.com",
                    Aesthetics = Aesthetics.Clean,
                    IsSubdomain = false,
                    IsMultilingual = false,
                };

                websiteRepo.Add(website);
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
