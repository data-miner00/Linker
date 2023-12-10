namespace Linker.Data.SQLite
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using EnsureThat;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    /// <summary>
    /// A repository responsible for Website entity.
    /// </summary>
    public class WebsiteRepository : IWebsiteRepository
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
        public async Task AddAsync(Website item, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

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
                    ModifiedAt,
                    CreatedBy
                ) VALUES (
                    @Id,
                    @Url,
                    @Category,
                    @Description,
                    @Language,
                    @LastVisitAt,
                    @CreatedAt,
                    @ModifiedAt,
                    @CreatedBy
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

            await this.connection.ExecuteAsync(insertToLinksOperation, new
            {
                Id = randomId,
                item.Url,
                Category = item.Category.ToString(),
                item.Description,
                Language = item.Language.ToString(),
                item.LastVisitAt,
                item.CreatedAt,
                item.ModifiedAt,
                item.CreatedBy,
            });

            await this.connection.ExecuteAsync(insertToWebsitesOperation, new
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
                var result = await this.connection.QueryAsync<Tag>(selectIdFromTagsQuery, new { Name = tag });

                if (!result.Any())
                {
                    var randomId2 = Guid.NewGuid().ToString();
                    await this.connection.ExecuteAsync(insertIntoTagsOperation, new
                    {
                        Id = randomId2,
                        Name = tag,
                        item.CreatedAt,
                        item.ModifiedAt,
                    });
                    await this.connection.ExecuteAsync(insertIntoLinkTagsOperation, new { LinkId = randomId, TagId = randomId2 });
                }
                else
                {
                    await this.connection.ExecuteAsync(insertIntoLinkTagsOperation, new { LinkId = randomId, TagId = result.FirstOrDefault()?.Id });
                }
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Website>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var websites = new List<Website>();

            var selectFromWebsitesQuery = @"SELECT * FROM Websites;";

            var partialWebsites = await this.connection.QueryAsync<PartialWebsite>(selectFromWebsitesQuery);

            foreach (var partialWebsite in partialWebsites)
            {
                var tags = new List<string>();

                var selectFromLinksQuery = @"SELECT * FROM Links WHERE Id = @Id;";
                var link = await this.connection.QueryFirstAsync<Link>(selectFromLinksQuery, new { Id = partialWebsite.LinkId });

                var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @Id;";
                var tagsz = await this.connection.QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { Id = partialWebsite.LinkId });
                foreach (var tagz in tagsz)
                {
                    var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";
                    var tag = await this.connection.QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
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
                    CreatedBy = link.CreatedBy,
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
        public async Task<Website> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var tags = new List<string>();

            var partialWebsite = await this.TryGetItemAsync(id);

            var selectFromLinksQuery = @"SELECT * FROM Links WHERE Id = @Id;";
            var link = await this.connection.QueryFirstAsync<Link>(selectFromLinksQuery, new { Id = id });

            var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @LinkId;";
            var tagsz = await this.connection.QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = id });

            foreach (var tagz in tagsz)
            {
                var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";
                var tag = await this.connection.QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
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
                CreatedBy = link.CreatedBy,
                Name = partialWebsite.Name,
                Domain = partialWebsite.Domain,
                Aesthetics = partialWebsite.Aesthetics,
                IsSubdomain = partialWebsite.IsSubdomain,
                IsMultilingual = partialWebsite.IsMultilingual,
            };

            return website;
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.TryGetItemAsync(id);

            var deleteFromWebsitesOperation = @"DELETE FROM Websites WHERE LinkId = @Id;";
            var deleteFromLinksOperation = @"DELETE FROM Links WHERE Id = @Id;";
            var deleteFromLinksTagsOperation = @"DELETE FROM Links_Tags Where LinkId = @Id;";

            await this.connection.ExecuteAsync(deleteFromWebsitesOperation, new { Id = id });
            await this.connection.ExecuteAsync(deleteFromLinksOperation, new { Id = id });
            await this.connection.ExecuteAsync(deleteFromLinksTagsOperation, new { Id = id });
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Website item, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.TryGetItemAsync(item.Id);

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

            await this.connection.ExecuteAsync(updateWebsitesOperation, new
            {
                item.Id,
                item.Name,
                item.Domain,
                Aesthetics = item.Aesthetics.ToString(),
                item.IsSubdomain,
                item.IsMultilingual,
            });

            await this.connection.ExecuteAsync(updateLinksOperation, new
            {
                item.Id,
                item.Url,
                Category = item.Category.ToString(),
                item.Description,
                Language = item.Language.ToString(),
                ModifiedAt = DateTime.Now,
            });
        }

        private async Task<PartialWebsite> TryGetItemAsync(string id)
        {
            var selectFromWebsitesQuery = @"SELECT * FROM Websites WHERE LinkId = @Id;";

            var partialWebsite = await this.connection.QueryFirstAsync<PartialWebsite>(selectFromWebsitesQuery, new { Id = id });

            return partialWebsite;
        }
    }
}
