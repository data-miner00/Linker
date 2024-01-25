namespace Linker.Data.SQLite
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;
    using EnsureThat;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    /// <summary>
    /// A repository for Youtube entity.
    /// </summary>
    public class YoutubeRepository : IYoutubeRepository
    {
        private readonly IDbConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="YoutubeRepository"/> class.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/>.</param>
        public YoutubeRepository(IDbConnection connection)
        {
            this.connection = EnsureArg.IsNotNull(connection, nameof(connection));
        }

        /// <inheritdoc/>
        public async Task AddAsync(Youtube item, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var randomId = Guid.NewGuid().ToString();

            var insertToYoutubeOperation = @"
                INSERT INTO Youtube (
                    Id,
                    Url,
                    Category,
                    Description,
                    Language,
                    Rating,
                    Name,
                    Youtuber,
                    Country,
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
                    @Youtuber,
                    @Country,
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

            await this.connection.ExecuteAsync(insertToYoutubeOperation, new
            {
                Id = randomId,
                item.Url,
                Category = item.Category.ToString(),
                item.Description,
                Language = item.Language.ToString(),
                Rating = item.Rating.ToString(),
                item.Name,
                item.Youtuber,
                item.Country,
                item.LastVisitAt,
                item.CreatedAt,
                item.ModifiedAt,
                item.CreatedBy,
            });

            foreach (var tag in item.Tags)
            {
                var result = this.connection.Query<Tag>(selectIdFromTagsQuery, new { Name = tag });

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
        public async Task<IEnumerable<Youtube>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var selectFromYoutubeQuery = @"SELECT * FROM Youtube;";

            var channels =
                await this.connection.QueryAsync<Youtube>(selectFromYoutubeQuery);

            foreach (var channel in channels)
            {
                var tags = new List<string>();

                var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @LinkId;";
                var tagsz = await this.connection.QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = channel.Id });

                foreach (var tagz in tagsz)
                {
                    var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";
                    var tag = await this.connection.QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
                    tags.Add(tag.Name);
                }

                channel.Tags = tags;
            }

            return channels;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Youtube>> GetAllByUserAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var channels = await this.GetAllAsync(cancellationToken).ConfigureAwait(false);

            return channels.Where(x => x.CreatedBy.Equals(userId, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc/>
        public async Task<Youtube> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var tags = new List<string>();

            var selectFromLYoutubeQuery = @"SELECT * FROM Youtube WHERE Id = @Id;";

            var youtube = await this.connection.QueryFirstAsync<Youtube>(selectFromLYoutubeQuery, new { Id = id });

            var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @LinkId;";

            var tagsz = await this.connection.QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = id });

            foreach (var tagz in tagsz)
            {
                var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";
                var tag = await this.connection.QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
                tags.Add(tag.Name);
            }

            youtube.Tags = tags;

            return youtube;
        }

        /// <inheritdoc/>
        public async Task<Youtube> GetByUserAsync(string userId, string linkId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var youtube = await this.GetByIdAsync(linkId, cancellationToken)
                .ConfigureAwait(false);

            if (!youtube.CreatedBy.Equals(userId, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("User not found");
            }

            return youtube;
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.TryGetItemAsync(id);

            var deleteFromYoutubeOperation = @"DELETE FROM Youtube WHERE Id = @Id;";
            var deleteFromLinksTagsOperation = @"DELETE FROM Links_Tags WHERE LinkId = @Id;";

            await this.connection.ExecuteAsync(deleteFromYoutubeOperation, new { Id = id });
            await this.connection.ExecuteAsync(deleteFromLinksTagsOperation, new { Id = id });
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Core.Models.Youtube item, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.TryGetItemAsync(item.Id);

            var updateYoutubeOperation = @"
                UPDATE Youtube
                SET
                    Name = @Name,
                    Youtuber = @Youtuber,
                    Country = @Country,
                    Url = @Url,
                    Category = @Category,
                    Description = @Description,
                    Language = @Language,
                    Rating = @Rating,
                    ModifiedAt = @ModifiedAt
                WHERE
                    Id = @Id;
            ";

            await this.connection.ExecuteAsync(updateYoutubeOperation, new
            {
                item.Id,
                item.Name,
                item.Youtuber,
                item.Country,
                item.Url,
                Category = item.Category.ToString(),
                item.Description,
                Language = item.Language.ToString(),
                Rating = item.Rating.ToString(),
                ModifiedAt = DateTime.Now,
            });
        }

        private async Task<Youtube> TryGetItemAsync(string id)
        {
            var selectFromChannelQuery = @"SELECT * FROM Youtube WHERE Id = @Id;";
            var channel = await this.connection.QueryFirstAsync<Youtube>(selectFromChannelQuery, new { Id = id });

            return channel;
        }
    }
}
