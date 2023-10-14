namespace Linker.TestCore.DataBuilders
{
    using System.Collections.Generic;
    using Linker.Core.ApiModels;
    using Linker.Core.Models;

    public sealed class CreateArticleRequestDataBuilder
    {
        private string title = "article title";
        private string url = "https://google.com";
        private Category category = default;
        private string description = "This is the description.";
        private IEnumerable<string> tags = new List<string>();
        private string author = "David";
        private Language language = Language.English;
        private Grammar grammar = default;
        private int year = 2000;
        private bool watchLater = false;

        public CreateArticleRequestDataBuilder WithTitle(string title)
        {
            this.title = title;
            return this;
        }

        public CreateArticleRequestDataBuilder WithUrl(string url)
        {
            this.url = url;
            return this;
        }

        public CreateArticleRequestDataBuilder WithCategory(Category category)
        {
            this.category = category;
            return this;
        }

        public CreateArticleRequestDataBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        public CreateArticleRequestDataBuilder WithTags(IEnumerable<string> tags)
        {
            this.tags = tags;
            return this;
        }

        public CreateArticleRequestDataBuilder WithAuthor(string author)
        {
            this.author = author;
            return this;
        }

        public CreateArticleRequestDataBuilder WithLanguage(Language language)
        {
            this.language = language;
            return this;
        }

        public CreateArticleRequestDataBuilder WithGrammar(Grammar grammar)
        {
            this.grammar = grammar;
            return this;
        }

        public CreateArticleRequestDataBuilder WithYear(int year)
        {
            this.year = year;
            return this;
        }

        public CreateArticleRequestDataBuilder WithWatchLater(bool watchLater)
        {
            this.watchLater = watchLater;
            return this;
        }

        public CreateArticleRequest Build()
        {
            return new CreateArticleRequest
            {
                Title = this.title,
                Url = this.url,
                Category = this.category,
                Description = this.description,
                Tags = this.tags,
                Author = this.author,
                Language = this.language,
                Grammar = this.grammar,
                Year = this.year,
                WatchLater = this.watchLater,
            };
        }
    }
}
