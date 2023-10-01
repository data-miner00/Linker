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
    using Linker.Core.Models.Dtos;
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
        public async Task AddAsync(Article item)
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

            var insertToArticleOperation = @"
                INSERT INTO Articles (
                    LinkId,
                    Title,
                    Author,
                    Year,
                    WatchLater,
                    Domain,
                    Grammar
                ) VALUES (
                    @LinkId,
                    @Title,
                    @Author,
                    @Year,
                    @WatchLater,
                    @Domain,
                    @Grammar
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
            });

            await this.connection.ExecuteAsync(insertToArticleOperation, new
            {
                LinkId = randomId,
                item.Title,
                item.Author,
                item.Year,
                item.WatchLater,
                item.Domain,
                Grammar = item.Grammar.ToString(),
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
        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            var articles = new List<Article>();

            var selectFromArticlesQuery = @"SELECT * FROM Articles;";

            var partialArticles = await this.connection.QueryAsync<PartialArticle>(selectFromArticlesQuery);

            foreach (var partialArticle in partialArticles)
            {
                var tags = new List<string>();

                var selectFromLinkQuery = @"SELECT * FROM Links WHERE Id = @Id;";
                var link = await this.connection.QueryFirstAsync<Link>(selectFromLinkQuery, new { Id = partialArticle.LinkId });

                var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @Id;";
                var tagsz = await this.connection.QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { Id = partialArticle.LinkId });
                foreach (var tagz in tagsz)
                {
                    var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";
                    var tag = await this.connection.QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
                    tags.Add(tag.Name);
                }

                var article = new Article
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
                    Title = partialArticle.Title,
                    Author = partialArticle.Author,
                    Year = partialArticle.Year,
                    WatchLater = partialArticle.WatchLater,
                    Domain = partialArticle.Domain,
                    Grammar = partialArticle.Grammar,
                };

                articles.Add(article);
            }

            return articles;
        }

        /// <inheritdoc/>
        public async Task<Article> GetByIdAsync(string id)
        {
            var tags = new List<string>();

            var partialArticle = await this.TryGetItemAsync(id);

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

            var article = new Article
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
                Title = partialArticle.Title,
                Author = partialArticle.Author,
                Year = partialArticle.Year,
                WatchLater = partialArticle.WatchLater,
                Domain = partialArticle.Domain,
                Grammar = partialArticle.Grammar,
            };

            return article;
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string id)
        {
            await this.TryGetItemAsync(id);

            var deleteFromArticlesOperation = @"DELETE FROM Articles WHERE LinkId = @Id;";
            var deleteFromLinksOperation = @"DELETE FROM Links WHERE Id = @Id;";
            var deleteFromLinksTagsOperation = @"DELETE FROM Links_Tags WHERE LinkId = @Id;";

            await this.connection.ExecuteAsync(deleteFromArticlesOperation, new { Id = id });
            await this.connection.ExecuteAsync(deleteFromLinksOperation, new { Id = id });
            await this.connection.ExecuteAsync(deleteFromLinksTagsOperation, new { Id = id });
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Article item)
        {
            await this.TryGetItemAsync(item.Id);

            var updateArticlesOperation = @"
                UPDATE Articles
                SET
                    Title = @Title,
                    Author = @Author,
                    Year = @Year,
                    WatchLater = @WatchLater,
                    Domain = @Domain,
                    Grammar = @Grammar
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

            await this.connection.ExecuteAsync(updateArticlesOperation, new
            {
                item.Id,
                item.Title,
                item.Author,
                item.Year,
                item.WatchLater,
                item.Domain,
                Grammar = item.Grammar.ToString(),
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

        private async Task<PartialArticle> TryGetItemAsync(string id)
        {
            var selectFromArticlesQuery = @"SELECT * FROM Articles WHERE LinkId = @Id;";
            var partialArticle = await this.connection.QueryFirstAsync<PartialArticle>(selectFromArticlesQuery, new { Id = id });

            return partialArticle;
        }
    }
}
