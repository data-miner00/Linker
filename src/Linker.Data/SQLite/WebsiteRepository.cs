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
        public async Task AddAsync(Website item, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var randomId = Guid.NewGuid().ToString();

            var insertToWebsitesOperation = @"
                INSERT INTO Websites (
                    Id,
                    Url,
                    Category,
                    Description,
                    Language,
                    Rating,
                    Name,
                    Domain,
                    Aesthetics,
                    IsSubdomain,
                    IsMultilingual,
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
                    @Rating,
                    @Name,
                    @Domain,
                    @Aesthetics,
                    @IsSubdomain,
                    @IsMultilingual,
                    @LastVisitAt,
                    @CreatedAt,
                    @ModifiedAt,
                    @CreatedBy
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

            await this.connection.ExecuteAsync(insertToWebsitesOperation, new
            {
                Id = randomId,
                item.Url,
                Category = item.Category.ToString(),
                item.Description,
                Language = item.Language.ToString(),
                Rating = item.Rating.ToString(),
                item.Name,
                item.Domain,
                Aesthetics = item.Aesthetics.ToString(),
                item.IsSubdomain,
                item.IsMultilingual,
                item.LastVisitAt,
                item.CreatedAt,
                item.ModifiedAt,
                item.CreatedBy,
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
        public async Task<IEnumerable<Website>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var selectFromWebsitesQuery = @"SELECT * FROM Websites;";

            var websites = await this.connection.QueryAsync<Website>(selectFromWebsitesQuery);

            foreach (var website in websites)
            {
                var tags = new List<string>();

                var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @LinkId;";
                var tagsz = await this.connection.QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = website.Id });

                foreach (var tagz in tagsz)
                {
                    var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";
                    var tag = await this.connection.QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
                    tags.Add(tag.Name);
                }

                website.Tags = tags;
            }

            return websites;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Website>> GetAllByUserAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var websites = await this.GetAllAsync(cancellationToken).ConfigureAwait(false);

            return websites.Where(x => x.CreatedBy.Equals(userId, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc/>
        public async Task<Website> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var tags = new List<string>();

            var selectFromWebsitesQuery = @"SELECT * FROM Websites WHERE Id = @Id;";
            var website = await this.connection.QueryFirstAsync<Website>(selectFromWebsitesQuery, new { Id = id });

            var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @LinkId;";
            var tagsz = await this.connection.QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = id });

            foreach (var tagz in tagsz)
            {
                var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";
                var tag = await this.connection.QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
                tags.Add(tag.Name);
            }

            website.Tags = tags;

            return website;
        }

        /// <inheritdoc/>
        public async Task<Website> GetByUserAsync(string userId, string linkId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var website = await this.GetByIdAsync(linkId, cancellationToken)
                .ConfigureAwait(false);

            if (!website.CreatedBy.Equals(userId, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("User not found");
            }

            return website;
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.TryGetItemAsync(id);

            var deleteFromWebsitesOperation = @"DELETE FROM Websites WHERE Id = @Id;";
            var deleteFromLinksTagsOperation = @"DELETE FROM Links_Tags Where LinkId = @Id;";

            await this.connection.ExecuteAsync(deleteFromWebsitesOperation, new { Id = id });
            await this.connection.ExecuteAsync(deleteFromLinksTagsOperation, new { Id = id });
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Website item, CancellationToken cancellationToken)
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
                    IsMultilingual = @IsMultilingual,
                    Url = @Url,
                    Category = @Category,
                    Description = @Description,
                    Language = @Language,
                    Rating = @Rating,
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
                item.Url,
                Category = item.Category.ToString(),
                item.Description,
                Language = item.Language.ToString(),
                Rating = item.Rating.ToString(),
                ModifiedAt = DateTime.Now,
            });
        }

        private async Task<Website> TryGetItemAsync(string id)
        {
            var selectFromWebsitesQuery = @"SELECT * FROM Websites WHERE Id = @Id;";

            var website = await this.connection.QueryFirstAsync<Website>(selectFromWebsitesQuery, new { Id = id });

            return website;
        }
    }
}
