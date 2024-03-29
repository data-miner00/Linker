﻿namespace Linker.ConsoleUI.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Linker.Common.Helpers;
    using Linker.Core.CsvModels;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    /// <summary>
    /// The article repository with csv as storage.
    /// </summary>
    public sealed class InMemoryCsvArticleRepository : ICsvArticleRepository
    {
        private readonly string pathToData;

        /// <summary>
        /// The in memory list that holds all articles.
        /// </summary>
        private List<Article> articles;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCsvArticleRepository"/> class.
        /// </summary>
        public InMemoryCsvArticleRepository()
        {
            var filePath = ConfigurationResolver.GetConfig("ArticleCsvPath");
            this.pathToData = Path.Combine(Environment.CurrentDirectory, filePath);

            this.articles = CsvHelper.Load<CsvArticle, Article, ArticleClassMap>(this.pathToData, this.CsvModelToModel);
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
            var link = this.articles.First(l => l.Id == item.Id)
                ?? throw new InvalidOperationException("Cannot find the link with id");

            link.Author = item.Author ?? link.Author;
            link.Url = item.Url ?? link.Url;
            link.Title = item.Title ?? link.Title;
            link.Description = item.Description ?? link.Description;
            link.Tags = item.Tags ?? link.Tags;

            link.Year = item.Year;
            link.Category = item.Category;
            link.Language = item.Language;
            link.Grammar = item.Grammar;

            link.ModifiedAt = DateTime.Now;
        }

        /// <inheritdoc/>
        public int Commit()
        {
            return CsvHelper.Save(this.pathToData, this.ModelToCsvModel, this.articles);
        }

        /// <inheritdoc/>
        public Article CsvModelToModel(CsvArticle csvModel)
        {
            return new Article
            {
                Id = csvModel.Id,
                Title = csvModel.Title,
                Author = csvModel.Author,
                Year = csvModel.Year,
                Url = csvModel.Url,
                Category = (Category)csvModel.Category,
                WatchLater = csvModel.WatchLater,
                Domain = csvModel.Domain,
                Description = csvModel.Description,
                Tags = csvModel.Tags.Split('|'),
                Language = (Language)csvModel.Language,
                Grammar = (Grammar)csvModel.Grammar,
                LastVisitAt = csvModel.LastVisitAt,
                CreatedAt = csvModel.CreatedAt,
                ModifiedAt = csvModel.ModifiedAt,
            };
        }

        /// <inheritdoc/>
        public CsvArticle ModelToCsvModel(Article model)
        {
            return new CsvArticle
            {
                Id = model.Id,
                Title = model.Title,
                Author = model.Author,
                Year = model.Year,
                Url = model.Url,
                Category = (int)model.Category,
                WatchLater = model.WatchLater,
                Domain = model.Domain,
                Description = model.Description,
                Tags = string.Join('|', model.Tags),
                Language = (int)model.Language,
                Grammar = (int)model.Grammar,
                LastVisitAt = model.LastVisitAt,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
            };
        }
    }
}
