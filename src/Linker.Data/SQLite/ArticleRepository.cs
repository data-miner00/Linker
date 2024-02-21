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
    /// A repository for Article entity.
    /// </summary>
    public class ArticleRepository : IArticleRepository
    {
        private readonly IDbConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleRepository"/> class.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> instance.</param>
        public ArticleRepository(IDbConnection connection)
        {
            this.connection = EnsureArg.IsNotNull(connection, nameof(connection));
        }

        /// <inheritdoc/>
        public async Task AddAsync(Article item, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(item);

            var randomId = Guid.NewGuid().ToString();

            var insertToArticlesOperation = @"
                INSERT INTO Articles (
                    Id,
                    Url,
                    Category,
                    Description,
                    Language,
                    Rating,
                    Title,
                    Author,
                    Year,
                    WatchLater,
                    Domain,
                    Grammar,
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
                    @Title,
                    @Author,
                    @Year,
                    @WatchLater,
                    @Domain,
                    @Grammar,
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

            await this.connection.ExecuteAsync(insertToArticlesOperation, new
            {
                Id = randomId,
                item.Url,
                Category = item.Category.ToString(),
                item.Description,
                Language = item.Language.ToString(),
                Rating = item.Rating.ToString(),
                item.Title,
                item.Author,
                item.Year,
                item.WatchLater,
                item.Domain,
                Grammar = item.Grammar.ToString(),
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
        public async Task<IEnumerable<Article>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var selectFromArticlesQuery = @"SELECT * FROM Articles;";

            var articles = await this.connection.QueryAsync<Article>(selectFromArticlesQuery);

            foreach (var article in articles)
            {
                var tags = new List<string>();

                var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @LinkId;";
                var tagsz = await this.connection.QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = article.Id });

                foreach (var tagz in tagsz)
                {
                    var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";
                    var tag = await this.connection.QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
                    tags.Add(tag.Name);
                }

                article.Tags = tags;
            }

            return articles;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Article>> GetAllByUserAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var articles = await this.GetAllAsync(cancellationToken).ConfigureAwait(false);

            return articles.Where(x => x.CreatedBy.Equals(userId, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc/>
        public async Task<Article> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var tags = new List<string>();

            var selectFromArticlesQuery = @"SELECT * FROM Articles WHERE Id = @Id;";

            var article = await this.connection.QueryFirstAsync<Article>(selectFromArticlesQuery, new { Id = id });

            var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @LinkId;";

            var tagsz = await this.connection.QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = id });

            foreach (var tagz in tagsz)
            {
                var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";
                var tag = await this.connection.QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
                tags.Add(tag.Name);
            }

            article.Tags = tags;

            return article;
        }

        /// <inheritdoc/>
        public async Task<Article> GetByUserAsync(string userId, string linkId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var article = await this.GetByIdAsync(linkId, cancellationToken)
                .ConfigureAwait(false);

            if (!article.CreatedBy.Equals(userId, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("User not found");
            }

            return article;
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.TryGetItemAsync(id);

            var deleteFromArticlesOperation = @"DELETE FROM Articles WHERE Id = @Id;";
            var deleteFromLinksTagsOperation = @"DELETE FROM Links_Tags WHERE LinkId = @Id;";

            await this.connection.ExecuteAsync(deleteFromArticlesOperation, new { Id = id });
            await this.connection.ExecuteAsync(deleteFromLinksTagsOperation, new { Id = id });
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Article item, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.TryGetItemAsync(item.Id);

            var updateArticlesOperation = @"
                UPDATE Articles
                SET
                    Title = @Title,
                    Author = @Author,
                    Year = @Year,
                    WatchLater = @WatchLater,
                    Domain = @Domain,
                    Grammar = @Grammar,
                    Url = @Url,
                    Category = @Category,
                    Description = @Description,
                    Language = @Language,
                    Rating = @Rating,
                    ModifiedAt = @ModifiedAt
                WHERE
                    Id = @Id;
            ";

            await this.connection.ExecuteAsync(updateArticlesOperation, new
            {
                item.Id,
                item.Title,
                item.Author,
                item.Year,
                item.WatchLater,
                item.Domain,
                Grammar = item.Grammar.ToString(),
                item.Url,
                Category = item.Category.ToString(),
                item.Description,
                Language = item.Language.ToString(),
                Rating = item.Rating.ToString(),
                ModifiedAt = DateTime.Now,
            });
        }

        private async Task<Article> TryGetItemAsync(string id)
        {
            var selectFromArticlesQuery = @"SELECT * FROM Articles WHERE Id = @Id;";
            var article = await this.connection.QueryFirstAsync<Article>(selectFromArticlesQuery, new { Id = id });

            return article;
        }
    }
}
