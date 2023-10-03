namespace Linker.TestCore.DataBuilders
{
    using System;
    using System.Collections.Generic;
    using Linker.Core.Models;

    public sealed class ArticleDataBuilder
    {
        private string id = "123";
        private string url = "https://www.google.com/blog";
        private Category category = Category.Education;
        private string description = "A long description";
        private IEnumerable<string> tags = new List<string> { "Tag" };
        private Language language = Language.English;
        private DateTime lastVisitAt = new DateTime(2023, 1, 1);
        private DateTime createdAt = new DateTime(2023, 1, 1);
        private DateTime modifiedAt = new DateTime(2023, 1, 1);
        private string title = "mock title";
        private string author = "mock author";
        private int year = 1999;
        private bool watchLater = false;
        private string domain = "www.google.com";
        private Grammar grammar = Grammar.Unknown;

        public ArticleDataBuilder WithId(string id)
        {
            this.id = id;
            return this;
        }

        public ArticleDataBuilder WithUrl(string url)
        {
            this.url = url;
            return this;
        }

        public ArticleDataBuilder WithCategory(Category category)
        {
            this.category = category;
            return this;
        }

        public ArticleDataBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        public ArticleDataBuilder WithTags(IEnumerable<string> tags)
        {
            this.tags = tags;
            return this;
        }

        public ArticleDataBuilder WithLanguage(Language language)
        {
            this.language = language;
            return this;
        }

        public ArticleDataBuilder WithLastVisitAt(DateTime lastVisitAt)
        {
            this.lastVisitAt = lastVisitAt;
            return this;
        }

        public ArticleDataBuilder WithCreatedAt(DateTime createdAt)
        {
            this.createdAt = createdAt;
            return this;
        }

        public ArticleDataBuilder WithModifiedAt(DateTime modifiedAt)
        {
            this.modifiedAt = modifiedAt;
            return this;
        }

        public ArticleDataBuilder WithTitle(string title)
        {
            this.title = title;
            return this;
        }

        public ArticleDataBuilder WithAuthor(string author)
        {
            this.author = author;
            return this;
        }

        public ArticleDataBuilder WithYear(int year)
        {
            this.year = year;
            return this;
        }

        public ArticleDataBuilder WithWatchLater(bool watchLater)
        {
            this.watchLater = watchLater;
            return this;
        }

        public ArticleDataBuilder WithDomain(string domain)
        {
            this.domain = domain;
            return this;
        }

        public ArticleDataBuilder WithGrammar(Grammar grammar)
        {
            this.grammar = grammar;
            return this;
        }

        public Article Build()
        {
            return new Article
            {
                Id = this.id,
                Url = this.url,
                Category = this.category,
                Description = this.description,
                Tags = this.tags,
                Language = this.language,
                LastVisitAt = this.lastVisitAt,
                CreatedAt = this.createdAt,
                ModifiedAt = this.modifiedAt,
                Title = this.title,
                Author = this.author,
                Year = this.year,
                WatchLater = this.watchLater,
                Domain = this.domain,
                Grammar = this.grammar,
            };
        }
    }
}
