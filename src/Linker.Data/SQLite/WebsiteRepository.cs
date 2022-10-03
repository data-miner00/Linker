namespace Linker.Data.SQLite
{
    using System.Data;
    using Dapper;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    public class WebsiteRepository : IRepository<Website>
    {
        private readonly IDbConnection connection;

        public WebsiteRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        /// <inheritdoc/>
        public void Add(Website item)
        {
            var query = @"
                INSERT INTO Websites (
                    Id,
                    Url,
                    Category,
                    Description,
                    Language,
                    Name,
                    Domain,
                    Aesthetics,
                    IsSubdomain,
                    IsMultilingual,
                    LastVisitAt,
                    CreatedAt,
                    ModifiedAt
                ) VALUES (
                    @Id,
                    @Url,
                    @Category,
                    @Description,
                    @Language,
                    @Name,
                    @Domain,
                    @Aesthetics,
                    @IsSubdomain,
                    @IsMultilingual,
                    @LastVisitAt,
                    @CreatedAt,
                    @ModifiedAt
                );
            ";

            this.connection.Execute(query, item);
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
            this.connection.Execute(query, new { Id = id, });
        }

        /// <inheritdoc/>
        public void Update(Website item)
        {
            var query = @"UPDATE Website SET Categpry = @Category WHERE Id = @Id;";

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
