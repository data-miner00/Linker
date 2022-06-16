namespace Linker.ConsoleUI.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Linker.ConsoleUI.Helpers;
    using Linker.Core.CsvModels;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    /// <summary>
    /// The article repository with csv as storage.
    /// </summary>
    public sealed class CsvArticleRepository : IArticleRepository
    {
        private readonly string pathToData;

        /// <summary>
        /// The in memory list that holds all articles.
        /// </summary>
        private List<Article> articles;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvArticleRepository"/> class.
        /// </summary>
        public CsvArticleRepository()
        {
            var filePath = ConfigurationResolver.GetConfig("ArticleCsvPath");
            this.pathToData = Path.Combine(Environment.CurrentDirectory, filePath);

            this.articles = CsvHelper.Load<CsvArticle, Article, ArticleClassMap>(this.pathToData, CsvArticleToArticle);
        }

        /// <summary>
        /// Converts the CsvArticle model to Article.
        /// </summary>
        /// <param name="csvArticle">The <see cref="CsvArticle"/> object.</param>
        /// <returns>The converted <see cref="Article"/> object.</returns>
        public static Article CsvArticleToArticle(CsvArticle csvArticle)
        {
            return new Article
            {
                Id = csvArticle.Id,
                Title = csvArticle.Title,
                Author = csvArticle.Author,
                Year = csvArticle.Year,
                Url = csvArticle.Url,
                Category = (Category)csvArticle.Category,
                WatchLater = csvArticle.WatchLater,
                Domain = csvArticle.Domain,
                Description = csvArticle.Description,
                Tags = csvArticle.Tags.Split('|'),
                Language = (Language)csvArticle.Language,
                Grammar = (Grammar)csvArticle.Grammar,
                LastVisitAt = csvArticle.LastVisitAt,
                CreatedAt = csvArticle.CreatedAt,
                ModifiedAt = csvArticle.ModifiedAt,
            };
        }

        /// <summary>
        /// Converts the Article model back to CsvArticle.
        /// </summary>
        /// <param name="article">The <see cref="Article"/> object.</param>
        /// <returns>The converted <see cref="CsvArticle"/> object.</returns>
        public static CsvArticle ArticleToCsvArticle(Article article)
        {
            return new CsvArticle
            {
                Id = article.Id,
                Title = article.Title,
                Author = article.Author,
                Year = article.Year,
                Url = article.Url,
                Category = (int)article.Category,
                WatchLater = article.WatchLater,
                Domain = article.Domain,
                Description = article.Description,
                Tags = string.Join('|', article.Tags),
                Language = (int)article.Language,
                Grammar = (int)article.Grammar,
                LastVisitAt = article.LastVisitAt,
                CreatedAt = article.CreatedAt,
                ModifiedAt = article.ModifiedAt,
            };
        }

        /// <inheritdoc/>
        public void Add(Article item)
        {
            var randomId = Guid.NewGuid().ToString();
            item.Id = randomId;
            this.articles.Add(item);
        }

        /// <inheritdoc/>
        public IEnumerable<Article> GetAll()
        {
            return from l in this.articles
                   orderby l.CreatedAt
                   select l;
        }

        /// <inheritdoc/>
        public Article GetById(string id)
        {
            var link = from l in this.articles
                       orderby l.CreatedAt
                       where l.Id == id
                       select l;

            return link.FirstOrDefault();
        }

        /// <inheritdoc/>
        public void Remove(string id)
        {
            this.articles = (from l in this.articles
                             where l.Id != id
                             select l).ToList();
        }

        /// <inheritdoc/>
        public void Update(Article item)
        {
            var _item = this.articles.FirstOrDefault(l => l.Id == item.Id);

            if (_item == null)
            {
                throw new InvalidOperationException("Cannot find the link with id");
            }

            _item.Author = item.Author ?? _item.Author;
            _item.Url = item.Url ?? _item.Url;
            _item.Title = item.Title ?? _item.Title;
            _item.Description = item.Description ?? _item.Description;
            _item.Tags = item.Tags ?? _item.Tags;

            _item.Year = item.Year;
            _item.Category = item.Category;
            _item.Language = item.Language;
            _item.Grammar = item.Grammar;

            _item.ModifiedAt = DateTime.Now;
        }

        /// <inheritdoc/>
        public int Commit()
        {
            return CsvHelper.Save(this.pathToData, ArticleToCsvArticle, this.articles);
        }
    }
}
