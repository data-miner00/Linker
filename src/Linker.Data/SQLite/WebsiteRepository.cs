namespace Linker.Data.SQLite
{
    using System.Data;
    using Dapper;
    using EnsureThat;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    /// <summary>
    /// A repository responsible for Website entity.
    /// </summary>
    public class WebsiteRepository : IRepository<Website>
    {
        private readonly IDbConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteRepository"/> class.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/>.</param>
        public WebsiteRepository(IDbConnection connection)
        {
            this.connection = EnsureArg.IsNotNull(connection, nameof(connection));
        }

        /// <inheritdoc/>
        public void Add(Website item)
        {
            var randomId = Guid.NewGuid().ToString();

            var insertToLinksOperation = @"
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

            var insertToWebsitesOperation = @"
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

            var selectIdFromTagsQuery = @"SELECT (Id) FROM Tags WHERE Name = @Name;";

            var insertIntoTagsOperation = @"
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

            var insertIntoLinkTagsOperation = @"
                INSERT INTO Links_Tags (
                    LinkId,
                    TagId
                ) VALUES (
                    @LinkId,
                    @TagId
                );
            ";

            this.connection.Execute(insertToLinksOperation, new
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

            this.connection.Execute(insertToWebsitesOperation, new
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
                var result = this.connection.Query<Tag>(selectIdFromTagsQuery, new { Name = tag });

                if (!result.Any())
                {
                    var randomId2 = Guid.NewGuid().ToString();
                    this.connection.Execute(insertIntoTagsOperation, new
                    {
                        Id = randomId2,
                        Name = tag,
                        item.CreatedAt,
                        item.ModifiedAt,
                    });
                    this.connection.Execute(insertIntoLinkTagsOperation, new { LinkId = randomId, TagId = randomId2 });
                }
                else
                {
                    this.connection.Execute(insertIntoLinkTagsOperation, new { LinkId = randomId, TagId = result.FirstOrDefault()?.Id });
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Website> GetAll()
        {
            var websites = new List<Website>();

            var selectFromWebsitesQuery = @"SELECT * FROM Websites;";

            var partialWebsites = this.connection.Query<PartialWebsite>(selectFromWebsitesQuery);

            foreach (var partialWebsite in partialWebsites)
            {
                var tags = new List<string>();

                var selectFromLinksQuery = @"SELECT * FROM Links WHERE Id = @Id;";
                var link = this.connection.QueryFirst<Link>(selectFromLinksQuery, new { Id = partialWebsite.LinkId });

                var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @Id;";
                var tagsz = this.connection.Query<LinkTagPair>(selectFromLinksTagsQuery, new { Id = partialWebsite.LinkId });
                foreach (var tagz in tagsz)
                {
                    var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";
                    var tag = this.connection.QueryFirst<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
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

            var selectFromWebsitesQuery = @"SELECT * FROM Websites WHERE LinkId = @Id;";

            var partialWebsite = this.connection.QueryFirst<PartialWebsite>(selectFromWebsitesQuery, new { Id = id });

            var selectFromLinksQuery = @"SELECT * FROM Links WHERE Id = @Id;";

            var link = this.connection.QueryFirst<Link>(selectFromLinksQuery, new { Id = id });

            var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @LinkId;";

            var tagsz = this.connection.Query<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = id });

            foreach (var tagz in tagsz)
            {
                var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";
                var tag = this.connection.QueryFirst<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
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
            var deleteFromWebsitesOperation = @"DELETE FROM Websites WHERE LinkId = @Id;";
            var deleteFromLinksOperation = @"DELETE FROM Links WHERE Id = @Id;";
            var deleteFromLinksTagsOperation = @"DELETE FROM Links_Tags Where LinkId = @Id;";

            this.connection.Execute(deleteFromWebsitesOperation, new { Id = id });
            this.connection.Execute(deleteFromLinksOperation, new { Id = id });
            this.connection.Execute(deleteFromLinksTagsOperation, new { Id = id });
        }

        /// <inheritdoc/>
        public void Update(Website item)
        {
            var updateWebsitesOperation = @"
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

            var updateLinksOperation = @"
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

            this.connection.Execute(updateWebsitesOperation, new
            {
                item.Id,
                item.Name,
                item.Domain,
                Aesthetics = item.Aesthetics.ToString(),
                item.IsSubdomain,
                item.IsMultilingual,
            });

            this.connection.Execute(updateLinksOperation, new
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
    }
}
