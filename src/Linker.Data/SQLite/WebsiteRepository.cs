namespace Linker.Data.SQLite
{
    using System.Data;
    using System.Security.Policy;
    using Dapper;
    using EnsureThat;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    public class WebsiteRepository : IRepository<Website>
    {
        private readonly IDbConnection connection;

        public WebsiteRepository(IDbConnection connection)
        {
            this.connection = EnsureArg.IsNotNull(connection);
        }

        /// <inheritdoc/>
        public void Add(Website item)
        {
            var randomId = Guid.NewGuid().ToString();

            var operation = @"
                INSERT INTO Links (
                    Id,
                    Url,
                    Category,
                    Description,
                    Language,
                    LastVisitAt,
                    CreatedAt,
                    ModifiedAt
                ) VALUES (
                    @Id,
                    @Url,
                    @Category,
                    @Description,
                    @Language,
                    @LastVisitAt,
                    @CreatedAt,
                    @ModifiedAt
                );
            ";

            var operation2 = @"
                INSERT INTO Websites (
                    LinkId,
                    Name,
                    Domain
                    Aesthetics
                    IsSubdomain
                    IsMultilingual
                ) VALUES (
                    @LinkId,
                    @Name,
                    @Domain,
                    @Aesthetics,
                    @IsSubdomain,
                    @IsMultilingual
                );
            ";

            var query = @"SELECT (Id) FROM Tags WHERE Name = @Name;";

            var operation3 = @"
                INSERT INTO Tags (
                    Id,
                    Name
                ) VALUES (
                    @Id,
                    @Name
                );
            ";

            var operation4 = @"
                INSERT INTO Links_Tags (
                    LinkId,
                    TagId
                ) VALUES (
                    @LinkId,
                    @TagId
                );
            ";

            this.connection.Execute(operation, new
            {
                Id = randomId,
                Url = item.Url,
                Category = item.Category.ToString(),
                Description = item.Description,
                Language = item.Language.ToString(),
                LastVisitAt = item.LastVisitAt,
                CreatedAt = item.CreatedAt,
                ModifiedAt = item.ModifiedAt,
            });

            this.connection.Execute(operation2, new
            {
                LinkId = randomId,
                item.Name,
                item.Domain,
                Aesthetics = item.Aesthetics.ToString(),
                item.IsSubdomain,
                item.IsMultilingual,
            });

            foreach (var tag in item.Tags)
            {
                var result = this.connection.Query(query, new { Name = tag });
                var randomId2 = Guid.NewGuid().ToString();

                if (!result.Any())
                {
                    this.connection.Execute(operation3, new
                    {
                        Id = randomId2,
                        Name = tag,
                    });
                }

                this.connection.Execute(operation4, new { LinkId = randomId, TagId = randomId2 });
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Website> GetAll()
        {
            var query = @"SELECT * FROM Websites;";
            var parameters = new DynamicParameters();

            var output = this.connection.Query<Website>(query, parameters);

            return output;
        }

        /// <inheritdoc/>
        public Website? GetById(string id)
        {
            var query = @"SELECT * FROM Website WHERE Id = " + id;

            var output = this.connection.Query<Website>(query);

            return output?.FirstOrDefault();
        }

        /// <inheritdoc/>
        public void Remove(string id)
        {
            var query = @"DELETE FROM Website WHERE Id = @Id;";
            this.connection.Execute(query, new { Id = id });
        }

        /// <inheritdoc/>
        public void Update(Website item)
        {
            var query = @"UPDATE Website SET Category = @Category WHERE Id = @Id;";

            this.Remove(item.Id);
            this.Add(item);
        }

        /// <inheritdoc/>
        public int Commit()
        {
            return 0;
        }
    }
}
