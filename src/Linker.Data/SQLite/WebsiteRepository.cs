namespace Linker.Data.SQLite
{
    using System.Data;
    using Dapper;
    using EnsureThat;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    public class WebsiteRepository : IRepository<Website>
    {
        private readonly IDbConnection connection;

        public WebsiteRepository(IDbConnection connection)
        {
            this.connection = EnsureArg.IsNotNull(connection, nameof(connection));
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
                    Domain,
                    Aesthetics,
                    IsSubdomain,
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
                    Name,
                    CreatedAt,
                    ModifiedAt
                ) VALUES (
                    @Id,
                    @Name,
                    @CreatedAt,
                    @ModifiedAt
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
                item.Url,
                Category = item.Category.ToString(),
                item.Description,
                Language = item.Language.ToString(),
                item.LastVisitAt,
                item.CreatedAt,
                item.ModifiedAt,
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
                var result = this.connection.Query<Tag>(query, new { Name = tag });

                if (!result.Any())
                {
                    var randomId2 = Guid.NewGuid().ToString();
                    this.connection.Execute(operation3, new
                    {
                        Id = randomId2,
                        Name = tag,
                        item.CreatedAt,
                        item.ModifiedAt,
                    });
                    this.connection.Execute(operation4, new { LinkId = randomId, TagId = randomId2 });
                }
                else
                {
                    this.connection.Execute(operation4, new { LinkId = randomId, TagId = result.FirstOrDefault()?.Id });
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Website> GetAll()
        {
            var websites = new List<Website>();

            var query = @"SELECT * FROM Websites;";

            var partialWebsites = this.connection.Query<PartialWebsite>(query);

            foreach (var partialWebsite in partialWebsites)
            {
                var tags = new List<string>();

                var query2 = @"SELECT * FROM Links WHERE Id = @Id;";
                var link = this.connection.QueryFirst<Link>(query2, new { Id = partialWebsite.LinkId });

                var query3 = @"SELECT * FROM Links_Tags WHERE LinkId = @Id;";
                var tagsz = this.connection.Query<LinkTagPair>(query3, new { Id = partialWebsite.LinkId });
                foreach (var tagz in tagsz)
                {
                    var query4 = @"SELECT * FROM Tags WHERE Id = @Id;";
                    var tag = this.connection.QueryFirst<Tag>(query4, new { Id = tagz.TagId });
                    tags.Add(tag.Name);
                }

                var website = new Website
                {
                    Id = link.Id,
                    Url = link.Url,
                    Category = link.Category,
                    Description = link.Description,
                    Tags = tags,
                    Language = link.Language,
                    LastVisitAt = link.LastVisitAt,
                    CreatedAt = link.CreatedAt,
                    ModifiedAt = link.ModifiedAt,
                    Name = partialWebsite.Name,
                    Domain = partialWebsite.Domain,
                    Aesthetics = partialWebsite.Aesthetics,
                    IsSubdomain = partialWebsite.IsSubdomain,
                    IsMultilingual = partialWebsite.IsMultilingual,
                };

                websites.Add(website);
            }

            return websites;
        }

        /// <inheritdoc/>
        public Website GetById(string id)
        {
            var tags = new List<string>();

            var query = @"SELECT * FROM Websites WHERE LinkId = @Id;";

            var partialWebsite = this.connection.QueryFirst<PartialWebsite>(query, new { Id = id });

            var query2 = @"SELECT * FROM Links WHERE Id = @Id;";

            var link = this.connection.QueryFirst<Link>(query2, new { Id = id });

            var query3 = @"SELECT * FROM Links_Tags WHERE LinkId = @LinkId;";

            var tagsz = this.connection.Query<LinkTagPair>(query3, new { LinkId = id });

            foreach (var tagz in tagsz)
            {
                var query4 = @"SELECT * FROM Tags WHERE Id = @Id;";
                var tag = this.connection.QueryFirst<Tag>(query4, new { Id = tagz.TagId });
                tags.Add(tag.Name);
            }

            var website = new Website
            {
                Id = link.Id,
                Url = link.Url,
                Category = link.Category,
                Description = link.Description,
                Tags = tags,
                Language = link.Language,
                LastVisitAt = link.LastVisitAt,
                CreatedAt = link.CreatedAt,
                ModifiedAt = link.ModifiedAt,
                Name = partialWebsite.Name,
                Domain = partialWebsite.Domain,
                Aesthetics = partialWebsite.Aesthetics,
                IsSubdomain = partialWebsite.IsSubdomain,
                IsMultilingual = partialWebsite.IsMultilingual,
            };

            return website;
        }

        /// <inheritdoc/>
        public void Remove(string id)
        {
            var query = @"DELETE FROM Websites WHERE LinkId = @Id;";
            var query2 = @"DELETE FROM Links WHERE Id = @Id;";
            var query3 = @"DELETE FROM Links_Tags Where LinkId = @Id;";
            this.connection.Execute(query, new { Id = id });
            this.connection.Execute(query2, new { Id = id });
            this.connection.Execute(query3, new { Id = id });
        }

        /// <inheritdoc/>
        public void Update(Website item)
        {
            var query = @"
                UPDATE Websites
                SET
                    Name = @Name,
                    Domain = @Domain,
                    Aesthetics = @Aesthetics,
                    IsSubdomain = @IsSubdomain,
                    IsMultilingual = @IsMultilingual
                WHERE
                    LinkId = @Id;
            ";

            var query2 = @"
                UPDATE Links
                SET
                    Url = @Url,
                    Category = @Category,
                    Description = @Description,
                    Language = @Language,
                    ModifiedAt = @ModifiedAt
                WHERE
                    Id = @Id;
            ";

            this.connection.Execute(query, new
            {
                item.Id,
                item.Name,
                item.Domain,
                Aesthetics = item.Aesthetics.ToString(),
                item.IsSubdomain,
                item.IsMultilingual,
            });

            this.connection.Execute(query2, new
            {
                item.Id,
                item.Url,
                Category = item.Category.ToString(),
                item.Description,
                Language = item.Language.ToString(),
                ModifiedAt = DateTime.Now,
            });
        }

        public void AddLinkTag(string linkId, string tagId)
        {
            var query = @"
                INSERT INTO Links_Tags (
                    LinkId,
                    TagId
                ) VALUES (
                    @LinkId,
                    @TagId
                );
            ";

            this.connection.Execute(query, new { LinkId = linkId, TagId = tagId });
        }

        public void DeleteLinkTag(string linkId, string tagId)
        {
            var query = @"
                DELETE FROM Links_Tags
                WHERE
                    LinkId = @LinkId,
                    TagId = @TagId;
            ";

            this.connection.Execute(query, new { LinkId = linkId, TagId = tagId });
        }

        /// <inheritdoc/>
        public int Commit()
        {
            return 0;
        }
    }
}
